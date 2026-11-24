namespace ChartNivra.Rendering.Skia;

using ChartNivra.Core.Primitives;
using ChartNivra.Core.Themes;
using ChartNivra.Cartesian;
using ChartNivra.Rendering.Abstractions;

public class WaterfallChartRenderer : IChartRenderer
{
    private readonly WaterfallSeriesModel _model;

    public WaterfallChartRenderer(WaterfallSeriesModel model)
    {
        _model = model;
    }

    public void Render(IChartCanvas canvas, ChartRect bounds)
    {
        var theme = ChartTheme.Light;
        canvas.DrawRectangle(bounds, new ChartBrush(theme.BackgroundColor));

        var topMargin = 20.0;
        canvas.DrawText(_model.Name, new ChartPoint(bounds.Left + 20, bounds.Top + 30), theme.TextColor, 16f, isBold: true);
        topMargin = 50.0;

        var data = _model.Points;
        if (data.Count == 0) return;

        var plotArea = new ChartRect(bounds.Left + 60, bounds.Top + topMargin, bounds.Width - 80, bounds.Height - topMargin - 40);

        // Compute running totals
        var runningTotal = 0.0;
        var barHeights = new double[data.Count];
        var barColors = new ChartColor[data.Count];
        var barBottoms = new double[data.Count];

        for (int i = 0; i < data.Count; i++)
        {
            var p = data[i];
            if (p.Type == WaterfallType.Total)
            {
                barBottoms[i] = 0;
                barHeights[i] = runningTotal;
                barColors[i] = _model.TotalColor;
            }
            else if (p.Type == WaterfallType.Incremental)
            {
                barBottoms[i] = runningTotal;
                barHeights[i] = p.Value;
                barColors[i] = _model.PositiveColor;
                runningTotal += p.Value;
            }
            else // Decremental
            {
                barBottoms[i] = runningTotal + p.Value;
                barHeights[i] = -p.Value;
                barColors[i] = _model.NegativeColor;
                runningTotal += p.Value;
            }
        }

        // Find min/max for scaling
        double minVal = double.MaxValue, maxVal = double.MinValue;
        for (int i = 0; i < data.Count; i++)
        {
            var top = barBottoms[i] + barHeights[i];
            var bottom = barBottoms[i];
            if (top < minVal) minVal = top;
            if (top > maxVal) maxVal = top;
            if (bottom < minVal) minVal = bottom;
            if (bottom > maxVal) maxVal = bottom;
        }

        var range = maxVal - minVal;
        if (range < 1e-9) range = 1;

        var barWidth = plotArea.Width / data.Count * 0.6;

        for (int i = 0; i < data.Count; i++)
        {
            var x = plotArea.Left + (i + 0.5) * plotArea.Width / data.Count;
            var halfW = barWidth / 2;

            var topY = plotArea.Bottom - (barBottoms[i] + barHeights[i] - minVal) / range * plotArea.Height;
            var bottomY = plotArea.Bottom - (barBottoms[i] - minVal) / range * plotArea.Height;
            var height = Math.Abs(bottomY - topY);

            canvas.DrawRectangle(new ChartRect(x - halfW, Math.Min(topY, bottomY), barWidth, height), new ChartBrush(barColors[i]), new ChartPen(barColors[i], 1f));

            // Connecting line
            if (i > 0)
            {
                var prevX = plotArea.Left + (i - 0.5) * plotArea.Width / data.Count;
                var prevTopY = plotArea.Bottom - (barBottoms[i - 1] + barHeights[i - 1] - minVal) / range * plotArea.Height;
                canvas.DrawLine(new ChartPoint(prevX, prevTopY), new ChartPoint(x, topY), new ChartPen(theme.GridLineColor, 1f, true));
            }

            // Label
            var labelSize = canvas.MeasureText(data[i].Category, 10f);
            canvas.DrawText(data[i].Category, new ChartPoint(x - labelSize.Width / 2, plotArea.Bottom + 18), theme.SecondaryTextColor, 10f);
        }

        canvas.DrawRectangle(plotArea, null, new ChartPen(theme.AxisLineColor, 1f));
    }
}
