namespace ChartNivra.Rendering.Abstractions;

using ChartNivra.Core.Axes;
using ChartNivra.Core.Charts;
using ChartNivra.Core.Primitives;
using ChartNivra.Core.Themes;
using ChartNivra.Core.Transforms;

/// <summary>
/// Default 2D Cartesian chart renderer using the IChartCanvas abstraction.
/// </summary>
public class CartesianChartRenderer : IChartRenderer
{
    private readonly CartesianChartModel _model;

    public CartesianChartRenderer(CartesianChartModel model)
    {
        _model = model ?? throw new ArgumentNullException(nameof(model));
    }

    public void Render(IChartCanvas canvas, ChartRect bounds)
    {
        var theme = _model.Theme ?? ChartTheme.Dark;

        // 1. Draw Background
        canvas.DrawRectangle(bounds, new ChartBrush(theme.BackgroundColor));

        // 2. Measure Title
        var topMargin = 20.0;
        if (!string.IsNullOrEmpty(_model.Title))
        {
            canvas.DrawText(_model.Title, new ChartPoint(bounds.Left + 20, bounds.Top + 30), theme.TextColor, 16f, isBold: true);
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
        canvas.DrawRectangle(plotArea, new ChartBrush(theme.PlotAreaColor));

        if (_model.Series.Count == 0) return;

        // 4. Compute Data Ranges across all series
        var (xRange, yRange, niceMinY, niceMaxY, stepY) = _model.ComputeRanges();
        var transform = new CartesianTransform(xRange, yRange, plotArea);

        // 5. Draw Horizontal Grid lines & Y Axis labels
        for (var v = niceMinY; v <= niceMaxY + 1e-9; v += stepY)
        {
            var pt = transform.DataToPixel(xRange.Min, v);
            var endPt = new ChartPoint(plotArea.Right, pt.Y);
            canvas.DrawLine(pt, endPt, new ChartPen(theme.GridLineColor, 1f));

            var label = v.ToString("G4");
            var labelSize = canvas.MeasureText(label, 11f);
            canvas.DrawText(label, new ChartPoint(plotArea.Left - labelSize.Width - 8, pt.Y + 4), theme.SecondaryTextColor, 11f);
        }

        // 6. Clip to Plot Area and draw series
        canvas.Save();
        canvas.ClipRect(plotArea);

        for (var i = 0; i < _model.Series.Count; i++)
        {
            var s = _model.Series[i];
            var color = s.Color;
            if (color == ChartColor.Black || color == ChartColor.FromHex("#3B82F6"))
            {
                color = theme.DefaultPalette.GetColor(i);
            }

            if (s.Points.Count > 0)
            {
                var screenPoints = new List<ChartPoint>(s.Points.Count);
                foreach (var pt in s.Points)
                {
                    screenPoints.Add(transform.DataToPixel(pt.X, pt.Y));
                }

                var pen = new ChartPen(color, s.StrokeWidth);
                canvas.DrawLines(screenPoints, pen);

                if (s.ShowMarkers)
                {
                    var brush = new ChartBrush(color);
                    var markerBorder = new ChartPen(ChartColor.White, 1.5f);
                    foreach (var pt in screenPoints)
                    {
                        canvas.DrawCircle(pt, s.MarkerRadius, brush, markerBorder);
                    }
                }
            }
        }

        canvas.Restore();

        // 7. Draw Plot Border
        canvas.DrawRectangle(plotArea, null, new ChartPen(theme.AxisLineColor, 1.5f));
    }
}
