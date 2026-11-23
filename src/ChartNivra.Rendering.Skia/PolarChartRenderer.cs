namespace ChartNivra.Rendering.Skia
{
    using ChartNivra.Core.Primitives;
    using ChartNivra.Core.Themes;
    using ChartNivra.Rendering.Abstractions;

    public class PolarChartRenderer : IChartRenderer
    {
        private readonly PolarChartModel _model;

        public PolarChartRenderer(PolarChartModel model)
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
                for (int i = 0; i <= 36; i++)
                {
                    var angle = i * 2 * Math.PI / 36;
                    points.Add(new ChartPoint(centerX + ringRadius * Math.Cos(angle), centerY + ringRadius * Math.Sin(angle)));
                }
                canvas.DrawLines(points, new ChartPen(theme.GridLineColor, 1f));
            }

            // Draw radial lines
            for (int i = 0; i < 12; i++)
            {
                var angle = i * 2 * Math.PI / 12;
                var x = centerX + radius * Math.Cos(angle);
                var y = centerY + radius * Math.Sin(angle);
                canvas.DrawLine(new ChartPoint(centerX, centerY), new ChartPoint(x, y), new ChartPen(theme.GridLineColor, 1f));
            }

            // Draw series
            foreach (var series in _model.Series)
            {
                if (series.Points.Count == 0) continue;

                var points = new List<ChartPoint>();
                foreach (var pt in series.Points)
                {
                    var angle = pt.Angle * Math.PI / 180 - Math.PI / 2;
                    var r = radius * pt.Radius / _model.MaxRadius;
                    points.Add(new ChartPoint(centerX + r * Math.Cos(angle), centerY + r * Math.Sin(angle)));
                }
                points.Add(points[0]); // Close

                canvas.DrawLines(points, new ChartPen(series.Color, 2f));
            }
        }
    }

    public class PolarChartModel
    {
        public string Title { get; set; } = "Polar Chart";
        public double MaxRadius { get; set; } = 100;
        public int GridRings { get; set; } = 5;
        public List<PolarSeriesModel> Series { get; } = new();
    }

    public class PolarSeriesModel
    {
        public string Title { get; set; } = string.Empty;
        public ChartColor Color { get; set; } = ChartColor.FromHex("#00C096");
        public float FillOpacity { get; set; } = 0.3f;
        public List<PolarPoint> Points { get; } = new();
    }

    public readonly record struct PolarPoint(double Angle, double Radius);
}