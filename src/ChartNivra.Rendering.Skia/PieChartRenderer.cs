namespace ChartNivra.Rendering.Skia
{
    using ChartNivra.Core.Circular;
    using ChartNivra.Core.Primitives;
    using ChartNivra.Core.Themes;
    using ChartNivra.Pie;
    using ChartNivra.Rendering.Abstractions;

    /// <summary>
    /// Renders Pie and Donut charts.
    /// </summary>
    public class PieChartRenderer : IChartRenderer
    {
        private readonly PieChartModel _model;

        public PieChartRenderer(PieChartModel model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public void Render(IChartCanvas canvas, ChartRect bounds)
        {
            var theme = _model.Theme ?? ChartTheme.Light;
            canvas.DrawRectangle(bounds, new ChartBrush(theme.BackgroundColor));
            var top = ChartDrawing.DrawTitle(canvas, _model.Title, bounds, theme);

            var total = _model.TotalValue;
            if (total <= 0) return;

            const double legendHeight = 40;
            var availH = bounds.Height - top - legendHeight;
            var radius = (float)(Math.Min(bounds.Width - 40, availH) / 2 - 6);
            if (radius < 10) return;
            var center = new ChartPoint(bounds.Left + bounds.Width / 2, bounds.Top + top + availH / 2);
            var inner = radius * _model.HoleRadiusRatio;

            var legend = new List<(string, ChartColor)>();
            float angle = -90;
            for (var i = 0; i < _model.Slices.Count; i++)
            {
                var slice = _model.Slices[i];
                if (slice.Value <= 0) continue;
                var color = slice.Color == ChartColor.Transparent ? theme.DefaultPalette.GetColor(i) : slice.Color;
                legend.Add(($"{slice.Label}  {slice.Value / total * 100:F0}%", color));

                var sweep = (float)(slice.Value / total * 360);
                var mid = (angle + sweep / 2) * Math.PI / 180;
                var c = center;
                if (slice.IsExploded)
                    c = new ChartPoint(center.X + radius * 0.08 * Math.Cos(mid), center.Y + radius * 0.08 * Math.Sin(mid));

                canvas.DrawArc(c, radius, inner, angle, sweep, new ChartBrush(color), new ChartPen(theme.BackgroundColor, 2.5f));

                // Percentage label inside the slice when there is room
                if (sweep >= 18)
                {
                    var lr = inner > 0 ? (radius + inner) / 2 : radius * 0.65;
                    var text = $"{slice.Value / total * 100:F0}%";
                    var size = canvas.MeasureText(text, 12f, true);
                    canvas.DrawText(text, new ChartPoint(c.X + lr * Math.Cos(mid) - size.Width / 2, c.Y + lr * Math.Sin(mid) + size.Height / 2),
                        ChartColor.White, 12f, true);
                }
                angle += sweep;
            }

            if (_model is DonutChartModel donut && !string.IsNullOrEmpty(donut.CenterTitle))
            {
                var tColor = theme.TextColor;
                var ts = canvas.MeasureText(donut.CenterTitle, 12f);
                var hasSub = !string.IsNullOrEmpty(donut.CenterSubtitle);
                canvas.DrawText(donut.CenterTitle, new ChartPoint(center.X - ts.Width / 2, center.Y - (hasSub ? 8 : -4)), theme.SecondaryTextColor, 12f);
                if (hasSub)
                {
                    var ss = canvas.MeasureText(donut.CenterSubtitle!, 22f, true);
                    canvas.DrawText(donut.CenterSubtitle!, new ChartPoint(center.X - ss.Width / 2, center.Y + 18), tColor, 22f, true);
                }
            }

            ChartDrawing.DrawLegend(canvas, legend, bounds, theme);
        }
    }
}