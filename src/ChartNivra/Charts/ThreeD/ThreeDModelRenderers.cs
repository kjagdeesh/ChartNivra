namespace ChartNivra.ThreeD;

using System.Numerics;
using ChartNivra.Core.Primitives;
using ChartNivra.Core.Themes;
using ChartNivra.Core.ThreeD;
using ChartNivra.Rendering.Abstractions;

/// <summary>Base helpers shared by the concrete, model-backed 3D sample renderers.</summary>
internal static class ThreeDRenderSupport
{
    public static void DrawBackground(IChartCanvas canvas, ChartRect bounds, string title, ChartTheme theme)
    {
        canvas.DrawRectangle(bounds, new ChartBrush(theme.BackgroundColor));
        canvas.DrawText(title, new ChartPoint(bounds.Left + 20, bounds.Top + 30), theme.TextColor, 16, true);
    }

    public static ChartPoint Project(Camera3D camera, Vector3 point, ChartRect bounds) =>
        camera.Project(new Vector3D(point.X, point.Y, point.Z), new ChartRect(bounds.Left + 12, bounds.Top + 44, bounds.Width - 24, bounds.Height - 56));

    public static ChartColor Blend(ChartColor low, ChartColor high, double amount) => new(
        (byte)(low.R + (high.R - low.R) * amount),
        (byte)(low.G + (high.G - low.G) * amount),
        (byte)(low.B + (high.B - low.B) * amount));

    public static void DrawAxisGuide(IChartCanvas canvas, ChartRect bounds, string x, string y, string z, ChartTheme theme)
    {
        var color = theme.SecondaryTextColor;
        var origin = new ChartPoint(bounds.Left + 44, bounds.Bottom - 42);
        var xEnd = new ChartPoint(origin.X + 55, origin.Y);
        var yEnd = new ChartPoint(origin.X, origin.Y - 42);
        var zEnd = new ChartPoint(origin.X + 30, origin.Y - 24);
        canvas.DrawLine(origin, xEnd, new ChartPen(ChartColor.FromHex("#38BDF8"), 1.5f));
        canvas.DrawLine(origin, yEnd, new ChartPen(ChartColor.FromHex("#A78BFA"), 1.5f));
        canvas.DrawLine(origin, zEnd, new ChartPen(ChartColor.FromHex("#34D399"), 1.5f));
        canvas.DrawText(x, new ChartPoint(xEnd.X + 4, xEnd.Y + 4), color, 9, true);
        canvas.DrawText(y, new ChartPoint(yEnd.X - 4, yEnd.Y - 5), color, 9, true);
        canvas.DrawText(z, new ChartPoint(zEnd.X + 3, zEnd.Y - 4), color, 9, true);
    }

    public static void DrawCallout(IChartCanvas canvas, ChartRect bounds, ChartPoint anchor, string text, ChartColor accent, ChartTheme theme, ICollection<ChartRect>? occupied = null)
    {
        var textSize = canvas.MeasureText(text, 10, true);
        var width = textSize.Width + 8;
        var height = textSize.Height + 7;
        var candidates = new List<ChartRect>();
        for (var offset = 0; offset <= 5; offset++)
        {
            foreach (var side in new[] { 1, -1 })
            foreach (var vertical in new[] { -1, 1 })
            {
                var x = side > 0 ? anchor.X + 18 : anchor.X - 18 - width;
                var y = vertical < 0 ? anchor.Y - 14 - height - offset * (height + 4) : anchor.Y + 14 + offset * (height + 4);
                x = Math.Clamp(x, bounds.Left + 4, Math.Max(bounds.Left + 4, bounds.Right - width - 4));
                y = Math.Clamp(y, bounds.Top + 36, Math.Max(bounds.Top + 36, bounds.Bottom - height - 4));
                candidates.Add(new ChartRect(x, y, width, height));
            }
        }
        var rect = candidates.OrderBy(candidate => occupied?.Count(existing => Intersects(existing, candidate)) ?? 0)
            .ThenBy(candidate => Math.Pow(Math.Clamp(anchor.X, candidate.Left, candidate.Right) - anchor.X, 2) + Math.Pow(Math.Clamp(anchor.Y, candidate.Top, candidate.Bottom) - anchor.Y, 2)).First();
        occupied?.Add(rect);
        var lineEnd = new ChartPoint(Math.Clamp(anchor.X, rect.Left, rect.Right), Math.Clamp(anchor.Y, rect.Top, rect.Bottom));
        canvas.DrawLine(anchor, lineEnd, new ChartPen(accent.WithAlpha(220), 1));
        canvas.DrawRoundRect(rect, 4, new ChartBrush(theme.PlotAreaColor), new ChartPen(accent.WithAlpha(180), 1));
        canvas.DrawText(text, new ChartPoint(rect.Left + 4, rect.Top + textSize.Height + 1), theme.TextColor, 10, true);
    }

    private static bool Intersects(ChartRect first, ChartRect second) => first.Left < second.Right && first.Right > second.Left && first.Top < second.Bottom && first.Bottom > second.Top;
}

/// <summary>Renderer for <see cref="ColumnChart3DModel"/> using depth-sorted shaded cuboid faces.</summary>
public sealed class ColumnChart3DRenderer : IChartRenderer
{
    public ChartTheme Theme { get; set; } = ChartTheme.Light;
    private readonly ColumnChart3DModel _model;
    public ColumnChart3DRenderer(ColumnChart3DModel model) => _model = model;

    public void Render(IChartCanvas canvas, ChartRect bounds)
    {
        ThreeDRenderSupport.DrawBackground(canvas, bounds, _model.Title, Theme);
        var faces = new List<(Triangle3D Triangle, float Depth)>();
        var callouts = new List<ChartRect>();
        foreach (var point in _model.Points)
        {
            var min = new Vector3((float)(point.X - point.Width / 2), 0, (float)(point.Z - point.Depth / 2));
            var max = new Vector3((float)(point.X + point.Width / 2), (float)point.Y, (float)(point.Z + point.Depth / 2));
            var color = point.Color ?? _model.DefaultColor;
            foreach (var face in Column3DMeshGenerator.CreateCuboid(min, max, color))
                faces.Add((face, face.AverageDepth(_model.Camera.GetViewMatrix())));
        }
        foreach (var item in faces.OrderBy(p => p.Depth))
        {
            var triangle = item.Triangle;
            var color = _model.EnableShading ? new Light3D().ApplyLighting(triangle.Color, triangle.Normal) : triangle.Color;
            var polygon = new[]
            {
                ThreeDRenderSupport.Project(_model.Camera, triangle.V0, bounds),
                ThreeDRenderSupport.Project(_model.Camera, triangle.V1, bounds),
                ThreeDRenderSupport.Project(_model.Camera, triangle.V2, bounds)
            };
            canvas.DrawPolygon(polygon, new ChartBrush(color), _model.ShowWireframe ? new ChartPen(color.WithAlpha(190), 1) : null);
        }
        foreach (var point in _model.Points)
        {
            if (string.IsNullOrWhiteSpace(point.Label)) continue;
            var labelAt = ThreeDRenderSupport.Project(_model.Camera, new Vector3((float)point.X, (float)point.Y, (float)point.Z), bounds);
            ThreeDRenderSupport.DrawCallout(canvas, bounds, labelAt, $"{point.Label} · {point.Y:0.0}", point.Color ?? _model.DefaultColor, Theme, callouts);
        }
        ThreeDRenderSupport.DrawAxisGuide(canvas, bounds, _model.XAxisTitle, _model.YAxisTitle, _model.ZAxisTitle, Theme);
    }
}

/// <summary>Renderer for a sampled <see cref="SurfaceChart3DModel"/> mesh.</summary>
public sealed class SurfaceChart3DRenderer : IChartRenderer
{
    public ChartTheme Theme { get; set; } = ChartTheme.Light;
    private readonly SurfaceChart3DModel _model;
    public SurfaceChart3DRenderer(SurfaceChart3DModel model) => _model = model;

    public void Render(IChartCanvas canvas, ChartRect bounds)
    {
        ThreeDRenderSupport.DrawBackground(canvas, bounds, _model.Title, Theme);
        var grid = _model.GenerateGrid();
        var min = double.MaxValue; var max = double.MinValue;
        foreach (var value in grid) { min = Math.Min(min, value); max = Math.Max(max, value); }
        var faces = new List<(Vector3 A, Vector3 B, Vector3 C, double Height)>();
        var stepX = (_model.MaxX - _model.MinX) / (_model.ResolutionX - 1);
        var stepY = (_model.MaxY - _model.MinY) / (_model.ResolutionY - 1);
        for (var x = 0; x < _model.ResolutionX - 1; x++)
        for (var y = 0; y < _model.ResolutionY - 1; y++)
        {
            var p00 = new Vector3((float)(_model.MinX + x * stepX), (float)grid[x, y], (float)(_model.MinY + y * stepY));
            var p10 = new Vector3((float)(_model.MinX + (x + 1) * stepX), (float)grid[x + 1, y], p00.Z);
            var p01 = new Vector3(p00.X, (float)grid[x, y + 1], (float)(_model.MinY + (y + 1) * stepY));
            var p11 = new Vector3(p10.X, (float)grid[x + 1, y + 1], p01.Z);
            faces.Add((p00, p10, p11, (p00.Y + p10.Y + p11.Y) / 3));
            faces.Add((p00, p11, p01, (p00.Y + p11.Y + p01.Y) / 3));
        }
        foreach (var face in faces.OrderBy(f => Vector4.Transform(new Vector4((f.A + f.B + f.C) / 3, 1), _model.Camera.GetViewMatrix()).Z))
        {
            var amount = Math.Clamp((face.Height - min) / Math.Max(0.00001, max - min), 0, 1);
            var color = ThreeDRenderSupport.Blend(_model.LowColor, _model.HighColor, amount);
            canvas.DrawPolygon(new[] { ThreeDRenderSupport.Project(_model.Camera, face.A, bounds), ThreeDRenderSupport.Project(_model.Camera, face.B, bounds), ThreeDRenderSupport.Project(_model.Camera, face.C, bounds) }, new ChartBrush(color), new ChartPen(color.WithAlpha(200), 0.7f));
        }
        ThreeDRenderSupport.DrawAxisGuide(canvas, bounds, _model.XAxisTitle, _model.ValueTitle, _model.YAxisTitle, Theme);
        ThreeDRenderSupport.DrawCallout(canvas, bounds, ThreeDRenderSupport.Project(_model.Camera, new Vector3((float)_model.MinX, (float)min, (float)_model.MinY), bounds), $"Low · {min:0.0}", _model.LowColor, Theme);
        ThreeDRenderSupport.DrawCallout(canvas, bounds, ThreeDRenderSupport.Project(_model.Camera, new Vector3((float)_model.MaxX, (float)max, (float)_model.MaxY), bounds), $"High · {max:0.0}", _model.HighColor, Theme);
    }
}

/// <summary>Renderer for projected, depth-sorted <see cref="ScatterChart3DModel"/> points.</summary>
public sealed class ScatterChart3DRenderer : IChartRenderer
{
    public ChartTheme Theme { get; set; } = ChartTheme.Light;
    private readonly ScatterChart3DModel _model;
    public ScatterChart3DRenderer(ScatterChart3DModel model) => _model = model;

    public void Render(IChartCanvas canvas, ChartRect bounds)
    {
        ThreeDRenderSupport.DrawBackground(canvas, bounds, _model.Title, Theme);
        var callouts = new List<ChartRect>();
        var points = _model.Points.OrderBy(p => Vector4.Transform(new Vector4((float)p.X, (float)p.Y, (float)p.Z, 1), _model.Camera.GetViewMatrix()).Z).ToArray();
        foreach (var point in points)
        {
            var screen = ThreeDRenderSupport.Project(_model.Camera, new Vector3((float)point.X, (float)point.Y, (float)point.Z), bounds);
            var color = point.Color ?? _model.DefaultColor;
            var radius = (float)Math.Clamp(point.Size, 2, 20);
            canvas.DrawCircle(screen, radius, new ChartBrush(color.WithAlpha(205)), new ChartPen(color, 1.5f));
            callouts.Add(new ChartRect(screen.X - radius - 2, screen.Y - radius - 2, radius * 2 + 4, radius * 2 + 4));
        }
        foreach (var point in points)
        {
            if (!string.IsNullOrWhiteSpace(point.Label))
            {
                var screen = ThreeDRenderSupport.Project(_model.Camera, new Vector3((float)point.X, (float)point.Y, (float)point.Z), bounds);
                ThreeDRenderSupport.DrawCallout(canvas, bounds, screen, $"{point.Label} · {point.X:0},{point.Y:0},{point.Z:0}", point.Color ?? _model.DefaultColor, Theme, callouts);
            }
        }
        ThreeDRenderSupport.DrawAxisGuide(canvas, bounds, _model.XAxisTitle, _model.YAxisTitle, _model.ZAxisTitle, Theme);
    }
}
