namespace ChartNivra.ThreeD;

using System.Numerics;
using ChartNivra.Core.Primitives;
using ChartNivra.Core.Themes;
using ChartNivra.Core.ThreeD;
using ChartNivra.Rendering.Abstractions;

/// <summary>Base helpers shared by the concrete, model-backed 3D sample renderers.</summary>
internal static class ThreeDRenderSupport
{
    public static void DrawBackground(IChartCanvas canvas, ChartRect bounds, string title)
    {
        canvas.DrawRectangle(bounds, new ChartBrush(ChartTheme.Dark.BackgroundColor));
        canvas.DrawText(title, new ChartPoint(bounds.Left + 20, bounds.Top + 30), ChartTheme.Dark.TextColor, 16, true);
    }

    public static ChartPoint Project(Camera3D camera, Vector3 point, ChartRect bounds) =>
        camera.Project(new Vector3D(point.X, point.Y, point.Z), new ChartRect(bounds.Left + 12, bounds.Top + 44, bounds.Width - 24, bounds.Height - 56));

    public static ChartColor Blend(ChartColor low, ChartColor high, double amount) => new(
        (byte)(low.R + (high.R - low.R) * amount),
        (byte)(low.G + (high.G - low.G) * amount),
        (byte)(low.B + (high.B - low.B) * amount));
}

/// <summary>Renderer for <see cref="ColumnChart3DModel"/> using depth-sorted shaded cuboid faces.</summary>
public sealed class ColumnChart3DRenderer : IChartRenderer
{
    private readonly ColumnChart3DModel _model;
    public ColumnChart3DRenderer(ColumnChart3DModel model) => _model = model;

    public void Render(IChartCanvas canvas, ChartRect bounds)
    {
        ThreeDRenderSupport.DrawBackground(canvas, bounds, _model.Title);
        var faces = new List<(Triangle3D Triangle, float Depth)>();
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
    }
}

/// <summary>Renderer for a sampled <see cref="SurfaceChart3DModel"/> mesh.</summary>
public sealed class SurfaceChart3DRenderer : IChartRenderer
{
    private readonly SurfaceChart3DModel _model;
    public SurfaceChart3DRenderer(SurfaceChart3DModel model) => _model = model;

    public void Render(IChartCanvas canvas, ChartRect bounds)
    {
        ThreeDRenderSupport.DrawBackground(canvas, bounds, _model.Title);
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
    }
}

/// <summary>Renderer for projected, depth-sorted <see cref="ScatterChart3DModel"/> points.</summary>
public sealed class ScatterChart3DRenderer : IChartRenderer
{
    private readonly ScatterChart3DModel _model;
    public ScatterChart3DRenderer(ScatterChart3DModel model) => _model = model;

    public void Render(IChartCanvas canvas, ChartRect bounds)
    {
        ThreeDRenderSupport.DrawBackground(canvas, bounds, _model.Title);
        foreach (var point in _model.Points.OrderBy(p => Vector4.Transform(new Vector4((float)p.X, (float)p.Y, (float)p.Z, 1), _model.Camera.GetViewMatrix()).Z))
        {
            var screen = ThreeDRenderSupport.Project(_model.Camera, new Vector3((float)point.X, (float)point.Y, (float)point.Z), bounds);
            var color = point.Color ?? _model.DefaultColor;
            canvas.DrawCircle(screen, (float)Math.Clamp(point.Size, 2, 20), new ChartBrush(color.WithAlpha(205)), new ChartPen(color, 1.5f));
        }
    }
}
