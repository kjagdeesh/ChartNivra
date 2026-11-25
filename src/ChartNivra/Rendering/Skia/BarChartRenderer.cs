namespace ChartNivra.Rendering.Skia;

using ChartNivra.Core.Charts;
using ChartNivra.Core.Primitives;
using ChartNivra.Core.Themes;
using ChartNivra.Rendering.Abstractions;

/// <summary>Grouped and stacked bar / column chart renderer.</summary>
public class BarChartRenderer : IChartRenderer
{
    private readonly BarChartModel _model;
        public ChartTheme Theme { get => _model.Theme; set => _model.Theme = value; }

    public BarChartRenderer(BarChartModel model) => _model = model ?? throw new ArgumentNullException(nameof(model));

    public void Render(IChartCanvas canvas, ChartRect bounds)
    {
        var theme = _model.Theme;
        canvas.DrawRectangle(bounds, new ChartBrush(theme.BackgroundColor));
        var top = ChartDrawing.DrawTitle(canvas, _model.Title, bounds, theme);
        var bottom = _model.ShowLegend && _model.Series.Count > 1 ? 62 : 36;
        var plot = new ChartRect(bounds.Left + 48, bounds.Top + top, Math.Max(10, bounds.Width - 68), Math.Max(10, bounds.Height - top - bottom));

        var count = _model.Categories.Count;
        if (count == 0 || _model.Series.Count == 0) return;

        var (_, niceMax, step) = _model.ComputeYRange();

        double Y(double v) => plot.Bottom - v / niceMax * plot.Height;

        for (var v = 0.0; v <= niceMax + 1e-9; v += step)
        {
            canvas.DrawLine(new ChartPoint(plot.Left, Y(v)), new ChartPoint(plot.Right, Y(v)), new ChartPen(theme.GridLineColor, 1f));
            var label = v.ToString("G4");
            var size = canvas.MeasureText(label, 11f);
            canvas.DrawText(label, new ChartPoint(plot.Left - size.Width - 8, Y(v) + 4), theme.SecondaryTextColor, 11f);
        }

        var slot = plot.Width / count;
        var groupWidth = slot * 0.7;
        var barWidth = _model.IsStacked ? groupWidth : groupWidth / _model.Series.Count;
        var radius = (float)Math.Min(6, barWidth / 3);

        for (var c = 0; c < count; c++)
        {
            var slotLeft = plot.Left + c * slot + (slot - groupWidth) / 2;
            double stackTop = 0;
            for (var si = 0; si < _model.Series.Count; si++)
            {
                var s = _model.Series[si];
                var color = s.Color == ChartColor.Transparent ? theme.DefaultPalette.GetColor(si) : s.Color;
                var v = s.Points.FirstOrDefault(p => (int)p.X == c).Y;
                if (v <= 0) continue;

                double yTop, yBottom, x;
                if (_model.IsStacked)
                {
                    yBottom = Y(stackTop); yTop = Y(stackTop + v); x = slotLeft; stackTop += v;
                }
                else
                {
                    yBottom = Y(0); yTop = Y(v); x = slotLeft + si * barWidth;
                }

                var w = barWidth - (_model.IsStacked ? 0 : 3);
                canvas.DrawRoundRect(new ChartRect(x, yTop, w, Math.Max(1, yBottom - yTop)), _model.IsStacked ? 0 : radius, new ChartBrush(color));

                if (_model.ShowValueLabels)
                {
                    var t = v.ToString("G4");
                    var size = canvas.MeasureText(t, 10f);
                    canvas.DrawText(t, new ChartPoint(x + (w - size.Width) / 2, yTop - 5), theme.SecondaryTextColor, 10f);
                }
            }

            var cat = _model.Categories[c];
            var cs = canvas.MeasureText(cat, 11f);
            canvas.DrawText(cat, new ChartPoint(plot.Left + c * slot + (slot - cs.Width) / 2, plot.Bottom + 18), theme.SecondaryTextColor, 11f);
        }

        canvas.DrawLine(new ChartPoint(plot.Left, plot.Bottom), new ChartPoint(plot.Right, plot.Bottom), new ChartPen(theme.AxisLineColor, 1f));

        if (_model.ShowLegend && _model.Series.Count > 1)
        {
            var items = _model.Series.Select((s, i) => (s.Title, s.Color == ChartColor.Transparent ? theme.DefaultPalette.GetColor(i) : s.Color)).ToList();
            ChartDrawing.DrawLegend(canvas, items, bounds, theme);
        }
    }
}
