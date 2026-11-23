namespace ChartNivra.Rendering.Skia
{
    using ChartNivra.Core.Financial;
    using ChartNivra.Core.Primitives;
    using ChartNivra.Core.Themes;
    using ChartNivra.Financial;
    using ChartNivra.Rendering.Abstractions;

    public class OhlcChartRenderer : IChartRenderer
    {
        private readonly FinancialChartModel _model;

        public OhlcChartRenderer(FinancialChartModel model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
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

            var data = _model.Candlesticks.Data;
            if (data.Count == 0) return;

            var volumeHeight = _model.ShowVolume ? bounds.Height * 0.2 : 0;
            var chartHeight = bounds.Height - topMargin - volumeHeight - 40;
            var chartTop = bounds.Top + topMargin;
            var volumeTop = chartTop + chartHeight + 10;

            var plotArea = new ChartRect(bounds.Left + 60, chartTop, bounds.Width - 80, chartHeight);
            var volumeArea = new ChartRect(bounds.Left + 60, volumeTop, bounds.Width - 80, volumeHeight);

            double minPrice = double.MaxValue, maxPrice = double.MinValue;
            foreach (var p in data)
            {
                if (p.Low < minPrice) minPrice = p.Low;
                if (p.High > maxPrice) maxPrice = p.High;
            }

            var priceRange = maxPrice - minPrice;
            if (priceRange < 1e-9) priceRange = 1;

            var niceMin = Math.Floor(minPrice / 10) * 10;
            var niceMax = Math.Ceiling(maxPrice / 10) * 10;
            var step = (niceMax - niceMin) / 5;

            for (var v = niceMin; v <= niceMax; v += step)
            {
                var y = chartTop + chartHeight - (v - minPrice) / priceRange * chartHeight;
                canvas.DrawLine(new ChartPoint(plotArea.Left, y), new ChartPoint(plotArea.Right, y), new ChartPen(theme.GridLineColor, 1f));
                var label = v.ToString("F0");
                var labelSize = canvas.MeasureText(label, 10f);
                canvas.DrawText(label, new ChartPoint(plotArea.Left - labelSize.Width - 8, y + 3), theme.SecondaryTextColor, 10f);
            }

            var labelStep = Math.Max(1, data.Count / 8);
            for (int i = 0; i < data.Count; i += labelStep)
            {
                var x = plotArea.Left + (i + 0.5) * plotArea.Width / data.Count;
                var label = data[i].Date.ToString("MM/dd");
                var labelSize = canvas.MeasureText(label, 10f);
                canvas.DrawText(label, new ChartPoint(x - labelSize.Width / 2, plotArea.Bottom + 18), theme.SecondaryTextColor, 10f);
            }

            DrawOhlcBars(canvas, data, plotArea, chartTop, chartHeight, minPrice, priceRange);
            DrawOverlays(canvas, data, plotArea, chartTop, chartHeight, minPrice, priceRange);

            if (_model.ShowVolume && volumeHeight > 0)
            {
                DrawVolume(canvas, data, plotArea, volumeTop, volumeHeight);
            }

            canvas.DrawRectangle(plotArea, null, new ChartPen(theme.AxisLineColor, 1f));
            if (_model.ShowVolume)
            {
                canvas.DrawRectangle(volumeArea, null, new ChartPen(theme.AxisLineColor, 1f));
            }
        }

        private void DrawOhlcBars(IChartCanvas canvas, IReadOnlyList<FinancialPoint> data, ChartRect plotArea, double chartTop, double chartHeight, double minPrice, double priceRange)
        {
            var barWidth = plotArea.Width / data.Count * 0.5;
            var halfWidth = barWidth / 2;

            for (int i = 0; i < data.Count; i++)
            {
                var p = data[i];
                var x = plotArea.Left + (i + 0.5) * plotArea.Width / data.Count;

                var highY = chartTop + chartHeight - (p.High - minPrice) / priceRange * chartHeight;
                var lowY = chartTop + chartHeight - (p.Low - minPrice) / priceRange * chartHeight;
                var openY = chartTop + chartHeight - (p.Open - minPrice) / priceRange * chartHeight;
                var closeY = chartTop + chartHeight - (p.Close - minPrice) / priceRange * chartHeight;

                var isBullish = p.Close >= p.Open;
                var color = isBullish ? _model.BullishColor : _model.BearishColor;

                canvas.DrawLine(new ChartPoint(x, highY), new ChartPoint(x, lowY), new ChartPen(color, 1f));
                canvas.DrawLine(new ChartPoint(x - halfWidth, openY), new ChartPoint(x, openY), new ChartPen(color, 1.5f));
                canvas.DrawLine(new ChartPoint(x, closeY), new ChartPoint(x + halfWidth, closeY), new ChartPen(color, 1.5f));
            }
        }

        private void DrawOverlays(IChartCanvas canvas, IReadOnlyList<FinancialPoint> data, ChartRect plotArea, double chartTop, double chartHeight, double minPrice, double priceRange)
        {
            foreach (var overlay in _model.Overlays)
            {
                if (overlay.Points.Count < 2) continue;

                var points = new List<ChartPoint>();
                foreach (var pt in overlay.Points)
                {
                    var idx = (int)pt.X;
                    if (idx >= 0 && idx < data.Count)
                    {
                        var x = plotArea.Left + (idx + 0.5) * plotArea.Width / data.Count;
                        var y = chartTop + chartHeight - (pt.Y - minPrice) / priceRange * chartHeight;
                        points.Add(new ChartPoint(x, y));
                    }
                }

                if (points.Count > 1)
                {
                    canvas.DrawLines(points, new ChartPen(overlay.Color, overlay.StrokeWidth));
                }
            }
        }

        private void DrawVolume(IChartCanvas canvas, IReadOnlyList<FinancialPoint> data, ChartRect plotArea, double volumeTop, double volumeHeight)
        {
            double maxVol = data.Max(d => d.Volume);
            if (maxVol <= 0) return;

            var barWidth = plotArea.Width / data.Count * 0.5;
            var halfWidth = barWidth / 2;

            for (int i = 0; i < data.Count; i++)
            {
                var p = data[i];
                var x = plotArea.Left + (i + 0.5) * plotArea.Width / data.Count;
                var volHeight = (p.Volume / maxVol) * volumeHeight * 0.9;
                var isBullish = p.Close >= p.Open;
                var color = isBullish ? _model.BullishColor : _model.BearishColor;

                canvas.DrawRectangle(new ChartRect(x - halfWidth, volumeTop + volumeHeight - volHeight, barWidth, volHeight), new ChartBrush(color.WithAlpha(128)));
            }
        }
    }
}