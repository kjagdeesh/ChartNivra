namespace ChartNivra.Core.Charts;

using ChartNivra.Core.Axes;
using ChartNivra.Core.Primitives;
using ChartNivra.Core.Series;
using ChartNivra.Core.Themes;
using ChartNivra.Core.Transforms;
using ChartNivra.Rendering.Abstractions;

/// <summary>
/// Cartesian chart model orchestrating 2D axes, series, layout, and rendering.
/// </summary>
public class CartesianChartModel : IChartRenderer
{
    public string Title { get; set; } = string.Empty;
    public ChartTheme Theme { get; set; } = ChartTheme.Dark;
    public List<LineSeriesModel> Series { get; } = new();

    public void Render(IChartCanvas canvas, ChartRect bounds)
    {
        // 1. Draw Background
        canvas.DrawRectangle(bounds, new ChartBrush(Theme.BackgroundColor));

        // 2. Measure Title
        var topMargin = 20.0;
        if (!string.IsNullOrEmpty(Title))
        {
            canvas.DrawText(Title, new ChartPoint(bounds.Left + 20, bounds.Top + 30), Theme.TextColor, 16f, isBold: true);
            topMargin = 50.0;
        }

        // 3. Define Plot Area
        var paddingLeft = 50.0;
        var paddingRight = 25.0;
        var paddingBottom = 40.0;

        var plotArea = new ChartRect(
            bounds.Left + paddingLeft,
            bounds.Top + topMargin,
            Math.Max(10, bounds.Width - paddingLeft - paddingRight),
            Math.Max(10, bounds.Height - topMargin - paddingBottom)
        );

        // Draw Plot Area Background
        canvas.DrawRectangle(plotArea, new ChartBrush(Theme.PlotAreaColor));

        if (Series.Count == 0) return;

        // 4. Compute Data Ranges across all series
        var minX = double.MaxValue; var maxX = double.MinValue;
        var minY = double.MaxValue; var maxY = double.MinValue;

        foreach (var s in Series)
        {
            var xr = s.GetXRange();
            var yr = s.GetYRange();
            if (xr.Min < minX) minX = xr.Min;
            if (xr.Max > maxX) maxX = xr.Max;
            if (yr.Min < minY) minY = yr.Min;
            if (yr.Max > maxY) maxY = yr.Max;
        }

        var (niceMinY, niceMaxY, stepY) = NiceScaleCalculator.Calculate(minY, maxY, 5);
        var yRange = new DataRange(niceMinY, niceMaxY);
        var xRange = new DataRange(minX, maxX);

        var transform = new CartesianTransform(xRange, yRange, plotArea);

        // 5. Draw Horizontal Grid lines & Y Axis labels
        for (var v = niceMinY; v <= niceMaxY + 1e-9; v += stepY)
        {
            var pt = transform.DataToPixel(xRange.Min, v);
            var endPt = new ChartPoint(plotArea.Right, pt.Y);
            canvas.DrawLine(pt, endPt, new ChartPen(Theme.GridLineColor, 1f));

            var label = v.ToString("G4");
            var labelSize = canvas.MeasureText(label, 11f);
            canvas.DrawText(label, new ChartPoint(plotArea.Left - labelSize.Width - 8, pt.Y + 4), Theme.SecondaryTextColor, 11f);
        }

        // 6. Clip to Plot Area and draw series
        canvas.Save();
        canvas.ClipRect(plotArea);

        for (var i = 0; i < Series.Count; i++)
        {
            var s = Series[i];
            if (s.Color == ChartColor.Black || s.Color == ChartColor.FromHex("#3B82F6"))
            {
                s.Color = Theme.DefaultPalette.GetColor(i);
            }
            s.Render(canvas, transform);
        }

        canvas.Restore();

        // 7. Draw Plot Border
        canvas.DrawRectangle(plotArea, null, new ChartPen(Theme.AxisLineColor, 1.5f));
    }
}
