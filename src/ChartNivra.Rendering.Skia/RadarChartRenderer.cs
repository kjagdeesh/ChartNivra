namespace ChartNivra.Rendering.Skia;

using ChartNivra.Core.Primitives;
using ChartNivra.Core.Themes;
using ChartNivra.Statistical;
using ChartNivra.Rendering.Abstractions;

public class RadarChartRenderer : IChartRenderer
{
    private readonly RadarChartModel _model;

    public RadarChartRenderer(RadarChartModel model)
    {
        _model = model;
    }

    public void Render(IChartCanvas canvas, ChartRect bounds)
    {
        var theme = ChartTheme.Dark;
        canvas.DrawRectangle(bounds, new ChartBrush(theme.BackgroundColor));

        var topMargin = 20.0;
        if (!string.IsNullOrEmpty(_model.Title))
        {
            canvas.DrawText(_model.Title, new ChartPoint(bounds.Left + 20, bounds.Top + 30), theme.TextColor, 16f, isBold: true);
            topMargin = 50.0;
        }

        var centerX = bounds.Left + bounds.Width / 2;
        var centerY = bounds.Top + topMargin + (bounds.Height - topMargin) / 2;
        var radius = Math.Min(bounds.Width, bounds.Height - topMargin) * 0.35;

        // Draw grid rings
        for (int r = 1; r <= _model.GridRings; r++)
        {
            var ringRadius = radius * r / _model.GridRings;
            var points = new List<ChartPoint>();
            for (int i = 0; i <= _model.Axes.Count; i++)
            {
                var angle = i * 2 * Math.PI / _model.Axes.Count - Math.PI / 2;
                points.Add(new ChartPoint(centerX + ringRadius * Math.Cos(angle), centerY + ringRadius * Math.Sin(angle)));
            }
            canvas.DrawLines(points, new ChartPen(theme.GridLineColor, 1f));
        }

        // Draw axis lines
        for (int i = 0; i < _model.Axes.Count; i++)
        {
            var angle = i * 2 * Math.PI / _model.Axes.Count - Math.PI / 2;
            var x = centerX + radius * Math.Cos(angle);
            var y = centerY + radius * Math.Sin(angle);
            canvas.DrawLine(new ChartPoint(centerX, centerY), new ChartPoint(x, y), new ChartPen(theme.GridLineColor, 1f));

            var label = _model.Axes[i];
            var labelSize = canvas.MeasureText(label, 11f);
            var lx = centerX + (radius + 15) * Math.Cos(angle) - labelSize.Width / 2;
            var ly = centerY + (radius + 15) * Math.Sin(angle) + labelSize.Height / 2;
            canvas.DrawText(label, new ChartPoint(lx, ly), theme.TextColor, 11f);
        }

        // Draw series
        for (int s = 0; s < _model.Series.Count; s++)
        {
            var series = _model.Series[s];
            var color = series.Color;
            var points = new List<ChartPoint>();

            for (int i = 0; i < _model.Axes.Count; i++)
            {
                var value = i < series.Values.Count ? series.Values[i] : 0;
                var normalizedRadius = radius * value / _model.MaxScale;
                var angle = i * 2 * Math.PI / _model.Axes.Count - Math.PI / 2;
                points.Add(new ChartPoint(centerX + normalizedRadius * Math.Cos(angle), centerY + normalizedRadius * Math.Sin(angle)));
            }
            points.Add(points[0]); // Close

            canvas.DrawLines(points, new ChartPen(color, 2f));

            if (series.FillOpacity > 0)
            {
                // Would need polygon fill
            }
        }
    }
}