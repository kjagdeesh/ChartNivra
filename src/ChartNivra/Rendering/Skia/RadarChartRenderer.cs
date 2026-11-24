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
        var theme = _model.Theme ?? ChartTheme.Light;
        canvas.DrawRectangle(bounds, new ChartBrush(theme.BackgroundColor));
        var top = ChartDrawing.DrawTitle(canvas, _model.Title, bounds, theme);

        var availH = bounds.Height - top - 40;
        var radius = (float)(Math.Min(bounds.Width - 40, availH) / 2 - 6);
        var center = new ChartPoint(bounds.Left + bounds.Width / 2, bounds.Top + top + availH / 2);

        // Draw grid rings
        for (int r = 1; r <= _model.GridRings; r++)
        {
            var ringRadius = radius * r / _model.GridRings;
            var points = new List<ChartPoint>();
            for (int i = 0; i <= _model.Axes.Count; i++)
            {
                var angle = i * 2 * Math.PI / _model.Axes.Count - Math.PI / 2;
                points.Add(new ChartPoint(center.X + ringRadius * Math.Cos(angle), center.Y + ringRadius * Math.Sin(angle)));
            }
            canvas.DrawLines(points, new ChartPen(theme.GridLineColor, 1f));
        }

        // Draw axis lines
        for (int i = 0; i < _model.Axes.Count; i++)
        {
            var angle = i * 2 * Math.PI / _model.Axes.Count - Math.PI / 2;
            var x = center.X + radius * Math.Cos(angle);
            var y = center.Y + radius * Math.Sin(angle);
            canvas.DrawLine(center, new ChartPoint(x, y), new ChartPen(theme.GridLineColor, 1f));

            var label = _model.Axes[i];
            var labelSize = canvas.MeasureText(label, 11f);
            var lx = center.X + (radius + 15) * Math.Cos(angle) - labelSize.Width / 2;
            var ly = center.Y + (radius + 15) * Math.Sin(angle) + labelSize.Height / 2;
            canvas.DrawText(label, new ChartPoint(lx, ly), theme.TextColor, 11f);
        }

        // Draw series
        var legend = new List<(string, ChartColor)>();
        for (int s = 0; s < _model.Series.Count; s++)
        {
            var series = _model.Series[s];
            var color = series.Color == ChartColor.Transparent ? theme.DefaultPalette.GetColor(s) : series.Color;
            legend.Add((series.Name, color));
            var points = new List<ChartPoint>();

            for (int i = 0; i < _model.Axes.Count; i++)
            {
                var value = i < series.Values.Count ? series.Values[i] : 0;
                var normalizedRadius = radius * value / _model.MaxScale;
                var angle = i * 2 * Math.PI / _model.Axes.Count - Math.PI / 2;
                points.Add(new ChartPoint(center.X + normalizedRadius * Math.Cos(angle), center.Y + normalizedRadius * Math.Sin(angle)));
            }
            points.Add(points[0]); // Close

            if (series.FillOpacity > 0 && points.Count > 2)
                canvas.DrawPolygon(points, new ChartBrush(color.WithAlpha((byte)(series.FillOpacity * 255))));
            canvas.DrawLines(points, new ChartPen(color, 2f));
            for (var i = 0; i < _model.Axes.Count; i++)
            {
                canvas.DrawCircle(points[i], 3.5f, new ChartBrush(ChartColor.White), new ChartPen(color, 1.5f));
                if (_model.Series.Count == 1)
                {
                    var text = (i < series.Values.Count ? series.Values[i] : 0).ToString("0");
                    var size = canvas.MeasureText(text, 9, true);
                    canvas.DrawText(text, new ChartPoint(points[i].X - size.Width / 2, points[i].Y - 7), color, 9, true);
                }
            }
        }

        if (_model.Series.Count > 1) ChartDrawing.DrawLegend(canvas, legend, bounds, theme);
    }
}
