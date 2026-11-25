namespace ChartNivra.Rendering.Skia
{
    using ChartNivra.Core.Primitives;
    using ChartNivra.Core.Themes;
    using ChartNivra.Rendering.Abstractions;

    public class ErrorBarsChartRenderer : IChartRenderer
    {
        private readonly ErrorBarsSeriesModel _model;
        public ChartTheme Theme { get; set; } = ChartTheme.Light;

        public ErrorBarsChartRenderer(ErrorBarsSeriesModel model)
        {
            _model = model;
        }

        public void Render(IChartCanvas canvas, ChartRect bounds)
        {
            var theme = Theme;
            canvas.DrawRectangle(bounds, new ChartBrush(theme.BackgroundColor));

            var topMargin = 20.0;
            canvas.DrawText("Error Bars Chart", new ChartPoint(bounds.Left + 20, bounds.Top + 30), theme.TextColor, 16f, isBold: true);
            topMargin = 50.0;

            var data = _model.Points;
            if (data.Count == 0) return;

            var plotArea = new ChartRect(bounds.Left + 60, bounds.Top + topMargin, bounds.Width - 80, bounds.Height - topMargin - 40);

            double minX = double.MaxValue, maxX = double.MinValue;
            double minY = double.MaxValue, maxY = double.MinValue;

            foreach (var p in data)
            {
                if (p.X < minX) minX = p.X;
                if (p.X > maxX) maxX = p.X;
                if (p.Y - p.ErrorMinus < minY) minY = p.Y - p.ErrorMinus;
                if (p.Y + p.ErrorPlus > maxY) maxY = p.Y + p.ErrorPlus;
            }

            var xRange = maxX - minX;
            var yRange = maxY - minY;
            if (xRange < 1e-9) xRange = 1;
            if (yRange < 1e-9) yRange = 1;

            foreach (var p in data)
            {
                var x = plotArea.Left + (p.X - minX) / xRange * plotArea.Width;
                var y = plotArea.Bottom - (p.Y - minY) / yRange * plotArea.Height;
                var errTop = plotArea.Bottom - (p.Y + p.ErrorPlus - minY) / yRange * plotArea.Height;
                var errBottom = plotArea.Bottom - (p.Y - p.ErrorMinus - minY) / yRange * plotArea.Height;

                // Error bar vertical line
                canvas.DrawLine(new ChartPoint(x, errTop), new ChartPoint(x, errBottom), new ChartPen(_model.ErrorBarColor, 1.5f));

                // Top cap
                canvas.DrawLine(new ChartPoint(x - _model.ErrorBarWidth / 2, errTop), new ChartPoint(x + _model.ErrorBarWidth / 2, errTop), new ChartPen(_model.ErrorBarColor, 1.5f));

                // Bottom cap
                canvas.DrawLine(new ChartPoint(x - _model.ErrorBarWidth / 2, errBottom), new ChartPoint(x + _model.ErrorBarWidth / 2, errBottom), new ChartPen(_model.ErrorBarColor, 1.5f));

                // Data point
                canvas.DrawCircle(new ChartPoint(x, y), 4f, new ChartBrush(_model.PointColor), new ChartPen(_model.PointColor, 1f));
            }

            canvas.DrawRectangle(plotArea, null, new ChartPen(theme.AxisLineColor, 1f));
        }
    }

    public class ErrorBarsSeriesModel
    {
        public string Title { get; set; } = string.Empty;
        public ChartColor Color { get; set; } = ChartColor.FromHex("#00C096");
        public ChartColor PointColor { get; set; } = ChartColor.FromHex("#00C096");
        public ChartColor ErrorBarColor { get; set; } = ChartColor.FromHex("#64748B");
        public float StrokeWidth { get; set; } = 2f;
        public float ErrorBarWidth { get; set; } = 8f;
        public List<ErrorBarPoint> Points { get; } = new();
    }

    public readonly record struct ErrorBarPoint(double X, double Y, double ErrorPlus, double ErrorMinus);
}
