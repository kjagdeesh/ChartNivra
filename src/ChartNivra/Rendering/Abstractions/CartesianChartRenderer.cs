namespace ChartNivra.Rendering.Abstractions;

using ChartNivra.Core.Charts;
using ChartNivra.Core.Primitives;
using ChartNivra.Core.Series;
using ChartNivra.Core.Themes;
using ChartNivra.Core.Transforms;

/// <summary>
/// Default 2D Cartesian renderer: lines, splines, area fills, markers, legend and category labels.
/// </summary>
public class CartesianChartRenderer : IChartRenderer
{
    private readonly CartesianChartModel _model;

    public CartesianChartRenderer(CartesianChartModel model)
    {
        _model = model ?? throw new ArgumentNullException(nameof(model));
    }

    /// <summary>Plot area used for the given bounds. Exposed so annotation overlays can align.</summary>
    public ChartRect GetPlotArea(ChartRect bounds)
    {
        var top = string.IsNullOrEmpty(_model.Title) ? 16 : 44;
        var bottom = _model.ShowLegend && _model.Series.Count > 1 ? 62 : 36;
        return new ChartRect(bounds.Left + 48, bounds.Top + top, Math.Max(10, bounds.Width - 48 - 20), Math.Max(10, bounds.Height - top - bottom));
    }

    public void Render(IChartCanvas canvas, ChartRect bounds)
    {
        var theme = _model.Theme ?? ChartTheme.Light;
        canvas.DrawRectangle(bounds, new ChartBrush(theme.BackgroundColor));
        ChartDrawing.DrawTitle(canvas, _model.Title, bounds, theme);

        var plot = GetPlotArea(bounds);
        if (_model.Series.Count == 0) return;

        var (xRange, yRange, niceMinY, niceMaxY, stepY) = _model.ComputeRanges();
        var transform = new CartesianTransform(xRange, yRange, plot);

        // Horizontal grid + Y labels
        for (var v = niceMinY; v <= niceMaxY + 1e-9; v += stepY)
        {
            var pt = transform.DataToPixel(xRange.Min, v);
            canvas.DrawLine(new ChartPoint(plot.Left, pt.Y), new ChartPoint(plot.Right, pt.Y), new ChartPen(theme.GridLineColor, 1f));
            var label = v.ToString("G4");
            var size = canvas.MeasureText(label, 11f);
            canvas.DrawText(label, new ChartPoint(plot.Left - size.Width - 8, pt.Y + 4), theme.SecondaryTextColor, 11f);
        }

        // X labels
        if (_model.XLabels.Count > 0)
        {
            var step = Math.Max(1, (int)Math.Ceiling(_model.XLabels.Count / Math.Max(1.0, plot.Width / 56)));
            for (var i = 0; i < _model.XLabels.Count; i += step)
            {
                var p = transform.DataToPixel(i, yRange.Min);
                var size = canvas.MeasureText(_model.XLabels[i], 11f);
                canvas.DrawText(_model.XLabels[i], new ChartPoint(p.X - size.Width / 2, plot.Bottom + 18), theme.SecondaryTextColor, 11f);
            }
        }

        canvas.Save();
        canvas.ClipRect(new ChartRect(plot.Left - 6, plot.Top - 6, plot.Width + 12, plot.Height + 12));

        var legend = new List<(string, ChartColor)>();
        for (var i = 0; i < _model.Series.Count; i++)
        {
            var s = _model.Series[i];
            var color = s.Color == ChartColor.FromHex("#3B82F6") || s.Color == ChartColor.Black
                ? theme.DefaultPalette.GetColor(i) : s.Color;
            legend.Add((string.IsNullOrEmpty(s.Title) ? $"Series {i + 1}" : s.Title, color));
            var data = s.GetPoints();
            if (data.Count == 0) continue;

            var screen = new List<ChartPoint>(data.Count);
            foreach (var pt in data) screen.Add(transform.DataToPixel(pt.X, pt.Y));
            var path = s.Smooth ? ChartDrawing.Smooth(screen) : screen;

            if (s.ShowArea && path.Count > 1)
            {
                var baseY = transform.DataToPixel(xRange.Min, Math.Max(yRange.Min, 0)).Y;
                var poly = new List<ChartPoint>(path.Count + 2) { new(path[0].X, baseY) };
                poly.AddRange(path);
                poly.Add(new ChartPoint(path[^1].X, baseY));
                canvas.DrawPolygon(poly, new ChartBrush(color.WithAlpha(s.AreaOpacity)));
            }

            canvas.DrawLines(path, new ChartPen(color, s.StrokeWidth));

            if (s.ShowMarkers)
            {
                var fill = new ChartBrush(theme.BackgroundColor);
                foreach (var pt in screen)
                    canvas.DrawCircle(pt, s.MarkerRadius, fill, new ChartPen(color, 2f));
            }

            if (s.ShowDataLabels)
                DrawDataLabels(canvas, s, screen, plot, theme);
        }

        canvas.Restore();
        canvas.DrawLine(new ChartPoint(plot.Left, plot.Bottom), new ChartPoint(plot.Right, plot.Bottom), new ChartPen(theme.AxisLineColor, 1f));

        if (_model.ShowLegend && legend.Count > 1) ChartDrawing.DrawLegend(canvas, legend, bounds, theme);
    }

    private static void DrawDataLabels(IChartCanvas canvas, LineSeriesModel series, IReadOnlyList<ChartPoint> points, ChartRect plot, ChartTheme theme)
    {
        var occupied = new List<ChartRect>();
        for (var i = 0; i < points.Count; i++)
        {
            var datum = series.DataPoints.Count > i ? series.DataPoints[i] : null;
            var text = datum?.ValueLabel ?? datum?.Label ?? series.GetPoints()[i].Y.ToString(series.DataLabelFormat);
            var size = canvas.MeasureText(text, 10, true);
            var above = new ChartRect(points[i].X - size.Width / 2 - 4, points[i].Y - size.Height - 12, size.Width + 8, size.Height + 6);
            var below = new ChartRect(points[i].X - size.Width / 2 - 4, points[i].Y + 7, size.Width + 8, size.Height + 6);
            var rect = occupied.Any(existing => Intersects(existing, above)) ? below : above;
            if (rect.Left < plot.Left || rect.Right > plot.Right || rect.Top < plot.Top || rect.Bottom > plot.Bottom) continue;
            canvas.DrawRoundRect(rect, 4, new ChartBrush(theme.BackgroundColor.WithAlpha(235)), new ChartPen(series.Color.WithAlpha(170), 1));
            canvas.DrawText(text, new ChartPoint(rect.Left + 4, rect.Top + size.Height + 1), theme.TextColor, 10, true);
            occupied.Add(rect);
        }
    }

    private static bool Intersects(ChartRect first, ChartRect second) =>
        first.Left < second.Right && first.Right > second.Left && first.Top < second.Bottom && first.Bottom > second.Top;
}
