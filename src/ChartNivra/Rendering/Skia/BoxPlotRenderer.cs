namespace ChartNivra.Rendering.Skia;

using ChartNivra.Core.Primitives;
using ChartNivra.Core.Themes;
using ChartNivra.Statistical;
using ChartNivra.Rendering.Abstractions;

public class BoxPlotRenderer : IChartRenderer
{
    private readonly BoxPlotSeriesModel _series;
    public ChartTheme Theme { get; set; }

    public BoxPlotRenderer(BoxPlotSeriesModel series, ChartTheme theme)
    {
        _series = series;
        Theme = theme;
    }

    public void Render(IChartCanvas canvas, ChartRect bounds)
    {
        canvas.DrawRectangle(bounds, new ChartBrush(Theme.BackgroundColor));
        var top = ChartDrawing.DrawTitle(canvas, "Box Plot", bounds, Theme);

        var data = _series.Data;
        if (data.Count == 0) return;

        var plotArea = new ChartRect(bounds.Left + 60, bounds.Top + top, bounds.Width - 80, bounds.Height - top - 40);

        double minVal = double.MaxValue, maxVal = double.MinValue;
        foreach (var p in data)
        {
            if (p.Min < minVal) minVal = p.Min;
            if (p.Max > maxVal) maxVal = p.Max;
        }

        var range = maxVal - minVal;
        if (range < 1e-9) range = 1;

        var boxWidth = plotArea.Width / data.Count * 0.6;

        for (int i = 0; i < data.Count; i++)
        {
            var p = data[i];
            var x = plotArea.Left + (i + 0.5) * plotArea.Width / data.Count;
            var halfW = boxWidth / 2;

            var minY = plotArea.Bottom - (p.Min - minVal) / range * plotArea.Height;
            var q1Y = plotArea.Bottom - (p.Q1 - minVal) / range * plotArea.Height;
            var medY = plotArea.Bottom - (p.Median - minVal) / range * plotArea.Height;
            var q3Y = plotArea.Bottom - (p.Q3 - minVal) / range * plotArea.Height;
            var maxY = plotArea.Bottom - (p.Max - minVal) / range * plotArea.Height;

            var color = Theme.DefaultPalette.GetColor(i);

            // Whiskers
            canvas.DrawLine(new ChartPoint(x, minY), new ChartPoint(x, maxY), new ChartPen(color, 1.5f));
            // Min/Max caps
            canvas.DrawLine(new ChartPoint(x - 4, minY), new ChartPoint(x + 4, minY), new ChartPen(color, 1.5f));
            canvas.DrawLine(new ChartPoint(x - 4, maxY), new ChartPoint(x + 4, maxY), new ChartPen(color, 1.5f));
            // Box with rounded corners
            canvas.DrawRoundRect(new ChartRect(x - halfW, q3Y, boxWidth, q1Y - q3Y), 2f, new ChartBrush(color.WithAlpha(80)), new ChartPen(color, 1.5f));
            // Median line
            canvas.DrawLine(new ChartPoint(x - halfW, medY), new ChartPoint(x + halfW, medY), new ChartPen(Theme.TextColor, 2f));

            var medianLabel = p.Median.ToString("0.0");
            var medianSize = canvas.MeasureText(medianLabel, 9, true);
            canvas.DrawText(medianLabel, new ChartPoint(x - medianSize.Width / 2, medY - 5), Theme.TextColor, 9, true);

            // Category label
            var labelSize = canvas.MeasureText(p.Category, 10f);
            canvas.DrawText(p.Category, new ChartPoint(x - labelSize.Width / 2, plotArea.Bottom + 18), Theme.SecondaryTextColor, 10f);
        }

        canvas.DrawLine(new ChartPoint(plotArea.Left, plotArea.Bottom), new ChartPoint(plotArea.Right, plotArea.Bottom), new ChartPen(Theme.AxisLineColor, 1f));
    }
}
