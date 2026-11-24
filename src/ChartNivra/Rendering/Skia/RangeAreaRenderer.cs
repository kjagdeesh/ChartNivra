namespace ChartNivra.Rendering.Skia;

using ChartNivra.Core.Primitives;
using ChartNivra.Core.Themes;
using ChartNivra.Cartesian;
using ChartNivra.Rendering.Abstractions;

public class RangeAreaRenderer : IChartRenderer
{
    private readonly RangeSeriesModel _model;

    public RangeAreaRenderer(RangeSeriesModel model)
    {
        _model = model;
    }

    public void Render(IChartCanvas canvas, ChartRect bounds)
    {
        var theme = ChartTheme.Light;
        canvas.DrawRectangle(bounds, new ChartBrush(theme.BackgroundColor));

        var topMargin = 20.0;
        canvas.DrawText("Range Area Chart", new ChartPoint(bounds.Left + 20, bounds.Top + 30), theme.TextColor, 16f, isBold: true);
        topMargin = 50.0;

        var data = _model.Points;
        if (data.Count == 0) return;

        var plotArea = new ChartRect(bounds.Left + 60, bounds.Top + topMargin, bounds.Width - 80, bounds.Height - topMargin - 40);

        double minX = double.MaxValue, maxX = double.MinValue;
        double minY = double.MaxValue, maxY = double.MinValue;

        foreach (var p in data)
        {
            if (p.X < minX) minX = p.X;
            if (p.X > maxX) maxX = p.X;
            if (p.Low < minY) minY = p.Low;
            if (p.High > maxY) maxY = p.High;
        }

        var xRange = maxX - minX;
        var yRange = maxY - minY;
        if (xRange < 1e-9) xRange = 1;
        if (yRange < 1e-9) yRange = 1;

        var upperPoints = new List<ChartPoint>();
        var lowerPoints = new List<ChartPoint>();

        for (int i = 0; i < data.Count; i++)
        {
            var p = data[i];
            var x = plotArea.Left + (p.X - minX) / xRange * plotArea.Width;
            var highY = plotArea.Bottom - (p.High - minY) / yRange * plotArea.Height;
            var lowY = plotArea.Bottom - (p.Low - minY) / yRange * plotArea.Height;

            upperPoints.Add(new ChartPoint(x, highY));
            lowerPoints.Add(new ChartPoint(x, lowY));
        }

        // Draw filled area (simplified - draw upper and lower lines)
        canvas.DrawLines(upperPoints, new ChartPen(_model.Color, 2f));
        var reversedLower = new List<ChartPoint>(lowerPoints);
        reversedLower.Reverse();
        canvas.DrawLines(reversedLower, new ChartPen(_model.Color, 2f));

        canvas.DrawRectangle(plotArea, null, new ChartPen(theme.AxisLineColor, 1f));
    }
}
