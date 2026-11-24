namespace ChartNivra.Rendering.Skia
{
    using ChartNivra.Core.Primitives;
    using ChartNivra.Core.Themes;
    using ChartNivra.Statistical;
    using ChartNivra.Rendering.Abstractions;

    public class HistogramRenderer : IChartRenderer
    {
        private readonly HistogramModel _model;

        public HistogramRenderer(HistogramModel model)
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

            var data = _model.Data;
            if (data == null || data.Count == 0) return;

            var plotArea = new ChartRect(bounds.Left + 60, bounds.Top + topMargin, bounds.Width - 80, bounds.Height - topMargin - 40);

            var (binWidth, bins, minVal, maxVal) = HistogramCalculator.ComputeBins(data, _model.BinCount);
            if (bins.Length == 0) return;

            var maxBin = bins.Max();
            if (maxBin == 0) return;

            var barWidth = plotArea.Width / bins.Length * 0.9;

            for (int i = 0; i < bins.Length; i++)
            {
                var x = plotArea.Left + i * plotArea.Width / bins.Length + (plotArea.Width / bins.Length - barWidth) / 2;
                var height = (bins[i] / (double)maxBin) * plotArea.Height * 0.9;
                var y = plotArea.Bottom - height;

                canvas.DrawRectangle(new ChartRect(x, y, barWidth, height), new ChartBrush(_model.BarColor.WithAlpha((byte)(255 * _model.BarOpacity))), new ChartPen(_model.BarColor, 1f));
            }

            canvas.DrawRectangle(plotArea, null, new ChartPen(theme.AxisLineColor, 1f));
        }
    }
}