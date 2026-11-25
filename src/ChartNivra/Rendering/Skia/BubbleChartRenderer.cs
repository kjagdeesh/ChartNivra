namespace ChartNivra.Rendering.Skia;

using ChartNivra.Core.Primitives;
using ChartNivra.Core.Themes;
using ChartNivra.Cartesian;
using ChartNivra.Rendering.Abstractions;

public class BubbleChartRenderer : IChartRenderer
{
    public ChartTheme Theme { get; set; } = ChartTheme.Light;
    private readonly BubbleSeriesModel _model;

    public BubbleChartRenderer(BubbleSeriesModel model)
    {
        _model = model;
    }

    public void Render(IChartCanvas canvas, ChartRect bounds)
    {
        var theme = Theme;
        canvas.DrawRectangle(bounds, new ChartBrush(theme.BackgroundColor));

        var topMargin = 20.0;
        canvas.DrawText("Bubble Chart", new ChartPoint(bounds.Left + 20, bounds.Top + 30), theme.TextColor, 16f, isBold: true);
        topMargin = 50.0;

        var data = _model.Points;
        if (data.Count == 0) return;

        var plotArea = new ChartRect(bounds.Left + 60, bounds.Top + topMargin, bounds.Width - 80, bounds.Height - topMargin - 40);

        double minX = double.MaxValue, maxX = double.MinValue;
        double minY = double.MaxValue, maxY = double.MinValue;
        double minSize = double.MaxValue, maxSize = double.MinValue;

        foreach (var p in data)
        {
            if (p.X < minX) minX = p.X;
            if (p.X > maxX) maxX = p.X;
            if (p.Y < minY) minY = p.Y;
            if (p.Y > maxY) maxY = p.Y;
            if (p.Size < minSize) minSize = p.Size;
            if (p.Size > maxSize) maxSize = p.Size;
        }

        var xRange = maxX - minX;
        var yRange = maxY - minY;
        var sizeRange = maxSize - minSize;
        if (xRange < 1e-9) xRange = 1;
        if (yRange < 1e-9) yRange = 1;
        if (sizeRange < 1e-9) sizeRange = 1;

        foreach (var p in data)
        {
            var x = plotArea.Left + (p.X - minX) / xRange * plotArea.Width;
            var y = plotArea.Bottom - (p.Y - minY) / yRange * plotArea.Height;
            var radius = _model.MinBubbleRadius + (p.Size - minSize) / sizeRange * (_model.MaxBubbleRadius - _model.MinBubbleRadius);

            canvas.DrawCircle(new ChartPoint(x, y), (float)radius, new ChartBrush(_model.Color.WithAlpha(180)), new ChartPen(_model.Color, 2f));

            if (!string.IsNullOrEmpty(p.Label))
            {
                var labelSize = canvas.MeasureText(p.Label, 10f);
                canvas.DrawText(p.Label, new ChartPoint(x - labelSize.Width / 2, y + radius + 12), theme.TextColor, 10f);
            }
        }

        canvas.DrawRectangle(plotArea, null, new ChartPen(theme.AxisLineColor, 1f));
    }
}
