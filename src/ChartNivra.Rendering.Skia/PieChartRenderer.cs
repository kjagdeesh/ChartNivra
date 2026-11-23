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
            var theme = _model.Theme ?? ChartTheme.Dark;

            // Draw background
            canvas.DrawRectangle(bounds, new ChartBrush(theme.BackgroundColor));

            // Draw title
            var topMargin = 20.0;
            if (!string.IsNullOrEmpty(_model.Title))
            {
                canvas.DrawText(_model.Title, new ChartPoint(bounds.Left + 20, bounds.Top + 30), theme.TextColor, 16f, isBold: true);
                topMargin = 50.0;
            }

            var centerX = bounds.Left + bounds.Width / 2;
            var centerY = bounds.Top + topMargin + (bounds.Height - topMargin) / 2;
            var radius = Math.Min(bounds.Width, bounds.Height - topMargin) * 0.35;

            var total = _model.TotalValue;
            if (total <= 0) return;

            double currentAngle = -90; // Start at top

            foreach (var slice in _model.Slices)
            {
                if (slice.Value <= 0) continue;

                var sweepAngle = (slice.Value / total) * 360;
                var sliceColor = slice.Color == ChartColor.Transparent ? theme.DefaultPalette.GetColor(0) : slice.Color;

                // Draw slice
                var startRad = currentAngle * Math.PI / 180;
                var endRad = (currentAngle + sweepAngle) * Math.PI / 180;

                var innerRadius = radius * _model.HoleRadiusRatio;
                var outerRadius = radius;

                if (slice.IsExploded)
                {
                    var midAngle = (currentAngle + sweepAngle / 2) * Math.PI / 180;
                    var explodeDist = radius * 0.1;
                    centerX += explodeDist * Math.Cos(midAngle);
                    centerY += explodeDist * Math.Sin(midAngle);
                }

                // Draw outer arc
                var points = new List<ChartPoint>();
                var segments = Math.Max(1, (int)(sweepAngle / 5));
                for (int i = 0; i <= segments; i++)
                {
                    var angle = currentAngle + (sweepAngle * i / segments);
                    var rad = angle * Math.PI / 180;
                    points.Add(new ChartPoint(
                        centerX + outerRadius * Math.Cos(rad),
                        centerY + outerRadius * Math.Sin(rad)));
                }

                // Draw inner arc (for donut)
                if (innerRadius > 0)
                {
                    for (int i = segments; i >= 0; i--)
                    {
                        var angle = currentAngle + (sweepAngle * i / segments);
                        var rad = angle * Math.PI / 180;
                        points.Add(new ChartPoint(
                            centerX + innerRadius * Math.Cos(rad),
                            centerY + innerRadius * Math.Sin(rad)));
                    }
                }

                // Close the path
                points.Add(points[0]);

                // Fill
                var brush = new ChartBrush(sliceColor);
                var pen = new ChartPen(theme.BackgroundColor, 2f);
                canvas.DrawLines(points, pen);
                // Note: Would need polygon fill - simplified for now

                // Draw label
                if (!string.IsNullOrEmpty(slice.Label))
                {
                    var labelAngle = (currentAngle + sweepAngle / 2) * Math.PI / 180;
                    var labelRadius = radius + 15;
                    var labelX = centerX + labelRadius * Math.Cos(labelAngle);
                    var labelY = centerY + labelRadius * Math.Sin(labelAngle);
                    var percent = (slice.Value / total * 100).ToString("F1");
                    canvas.DrawText($"{slice.Label} {percent}%", new ChartPoint(labelX, labelY), theme.TextColor, 11f);
                }

                currentAngle += sweepAngle;
            }

            // Draw center text for donut
            if (_model is DonutChartModel donut && !string.IsNullOrEmpty(donut.CenterTitle))
            {
                var text = donut.CenterTitle;
                var size = canvas.MeasureText(text, 18f, isBold: true);
                canvas.DrawText(text, new ChartPoint(centerX - size.Width / 2, centerY - 10), donut.CenterTextColor, 18f, isBold: true);

                if (!string.IsNullOrEmpty(donut.CenterSubtitle))
                {
                    var subSize = canvas.MeasureText(donut.CenterSubtitle, 14f);
                    canvas.DrawText(donut.CenterSubtitle, new ChartPoint(centerX - subSize.Width / 2, centerY + 12), donut.CenterTextColor, 14f);
                }
            }
        }
    }
}