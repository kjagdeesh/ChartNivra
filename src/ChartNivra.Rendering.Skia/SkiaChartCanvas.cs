namespace ChartNivra.Rendering.Skia;

using ChartNivra.Core.Primitives;
using ChartNivra.Rendering.Abstractions;
using SkiaSharp;

/// <summary>
/// SkiaSharp implementation of IChartCanvas for high-performance cross-platform 2D drawing.
/// </summary>
public sealed class SkiaChartCanvas : IChartCanvas
{
    private readonly SKCanvas _canvas;
    private readonly SKPaint _paint = new() { IsAntialias = true };

    public SkiaChartCanvas(SKCanvas canvas)
    {
        _canvas = canvas ?? throw new ArgumentNullException(nameof(canvas));
    }

    public void Clear(ChartColor color)
    {
        _canvas.Clear(new SKColor(color.R, color.G, color.B, color.A));
    }

    public void DrawLine(ChartPoint start, ChartPoint end, ChartPen pen)
    {
        _paint.Style = SKPaintStyle.Stroke;
        _paint.Color = new SKColor(pen.Color.R, pen.Color.G, pen.Color.B, pen.Color.A);
        _paint.StrokeWidth = pen.StrokeWidth;
        _canvas.DrawLine((float)start.X, (float)start.Y, (float)end.X, (float)end.Y, _paint);
    }

    public void DrawLines(IReadOnlyList<ChartPoint> points, ChartPen pen)
    {
        if (points.Count < 2) return;

        using var path = new SKPath();
        path.MoveTo((float)points[0].X, (float)points[0].Y);

        for (var i = 1; i < points.Count; i++)
        {
            path.LineTo((float)points[i].X, (float)points[i].Y);
        }

        _paint.Style = SKPaintStyle.Stroke;
        _paint.Color = new SKColor(pen.Color.R, pen.Color.G, pen.Color.B, pen.Color.A);
        _paint.StrokeWidth = pen.StrokeWidth;
        _canvas.DrawPath(path, _paint);
    }

    public void DrawRectangle(ChartRect rect, ChartBrush? brush, ChartPen? pen = null)
    {
        var skRect = new SKRect((float)rect.Left, (float)rect.Top, (float)rect.Right, (float)rect.Bottom);

        if (brush != null)
        {
            _paint.Style = SKPaintStyle.Fill;
            _paint.Color = new SKColor(brush.Color.R, brush.Color.G, brush.Color.B, brush.Color.A);
            _canvas.DrawRect(skRect, _paint);
        }

        if (pen != null)
        {
            _paint.Style = SKPaintStyle.Stroke;
            _paint.Color = new SKColor(pen.Color.R, pen.Color.G, pen.Color.B, pen.Color.A);
            _paint.StrokeWidth = pen.StrokeWidth;
            _canvas.DrawRect(skRect, _paint);
        }
    }

    public void DrawCircle(ChartPoint center, float radius, ChartBrush? brush, ChartPen? pen = null)
    {
        if (brush != null)
        {
            _paint.Style = SKPaintStyle.Fill;
            _paint.Color = new SKColor(brush.Color.R, brush.Color.G, brush.Color.B, brush.Color.A);
            _canvas.DrawCircle((float)center.X, (float)center.Y, radius, _paint);
        }

        if (pen != null)
        {
            _paint.Style = SKPaintStyle.Stroke;
            _paint.Color = new SKColor(pen.Color.R, pen.Color.G, pen.Color.B, pen.Color.A);
            _paint.StrokeWidth = pen.StrokeWidth;
            _canvas.DrawCircle((float)center.X, (float)center.Y, radius, _paint);
        }
    }

    public void DrawText(string text, ChartPoint position, ChartColor color, float fontSize, bool isBold = false)
    {
        if (string.IsNullOrEmpty(text)) return;

        _paint.Style = SKPaintStyle.Fill;
        _paint.Color = new SKColor(color.R, color.G, color.B, color.A);
        _paint.TextSize = fontSize;
        _paint.FakeBoldText = isBold;
        _canvas.DrawText(text, (float)position.X, (float)position.Y, _paint);
    }

    public ChartSize MeasureText(string text, float fontSize, bool isBold = false)
    {
        if (string.IsNullOrEmpty(text)) return ChartSize.Zero;

        _paint.TextSize = fontSize;
        _paint.FakeBoldText = isBold;
        var width = _paint.MeasureText(text);
        var bounds = new SKRect();
        _paint.MeasureText(text, ref bounds);
        return new ChartSize(width, bounds.Height);
    }

    public void Save() => _canvas.Save();
    public void Restore() => _canvas.Restore();

    public void ClipRect(ChartRect rect)
    {
        _canvas.ClipRect(new SKRect((float)rect.Left, (float)rect.Top, (float)rect.Right, (float)rect.Bottom));
    }

    public void Dispose()
    {
        _paint.Dispose();
    }
}
