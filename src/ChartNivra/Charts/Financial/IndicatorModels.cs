namespace ChartNivra.Financial;

using ChartNivra.Core.Financial;
using ChartNivra.Core.Primitives;
using ChartNivra.Core.Series;

/// <summary>
/// Bollinger Bands calculated result (Upper, Middle/SMA, Lower bands).
/// </summary>
public record BollingerBandResult(IReadOnlyList<ChartPoint> Upper, IReadOnlyList<ChartPoint> Middle, IReadOnlyList<ChartPoint> Lower);

/// <summary>
/// MACD indicator result (MACD Line, Signal Line, Histogram).
/// </summary>
public record MacdResult(IReadOnlyList<ChartPoint> MacdLine, IReadOnlyList<ChartPoint> SignalLine, IReadOnlyList<ChartPoint> Histogram);

/// <summary>
/// Extended technical indicators calculator for professional trading charts.
/// </summary>
public static class AdvancedIndicators
{
    public static BollingerBandResult CalculateBollingerBands(IReadOnlyList<FinancialPoint> data, int period = 20, double stdDevMultiplier = 2.0)
    {
        var middle = TechnicalIndicators.CalculateSma(data, period);
        var upper = new List<ChartPoint>(middle.Count);
        var lower = new List<ChartPoint>(middle.Count);

        for (var i = 0; i < middle.Count; i++)
        {
            var dataIndex = (int)middle[i].X;
            var sumSquares = 0.0;
            var mean = middle[i].Y;

            for (var j = dataIndex - period + 1; j <= dataIndex; j++)
            {
                var diff = data[j].Close - mean;
                sumSquares += diff * diff;
            }

            var stdDev = Math.Sqrt(sumSquares / period);
            upper.Add(new ChartPoint(dataIndex, mean + stdDevMultiplier * stdDev));
            lower.Add(new ChartPoint(dataIndex, mean - stdDevMultiplier * stdDev));
        }

        return new BollingerBandResult(upper, middle, lower);
    }

    public static IReadOnlyList<ChartPoint> CalculateRsi(IReadOnlyList<FinancialPoint> data, int period = 14)
    {
        if (data == null || data.Count <= period) return Array.Empty<ChartPoint>();

        var result = new List<ChartPoint>(data.Count - period);
        var gains = new double[data.Count];
        var losses = new double[data.Count];

        for (var i = 1; i < data.Count; i++)
        {
            var change = data[i].Close - data[i - 1].Close;
            if (change > 0) gains[i] = change;
            else losses[i] = -change;
        }

        var avgGain = 0.0;
        var avgLoss = 0.0;
        for (var i = 1; i <= period; i++)
        {
            avgGain += gains[i];
            avgLoss += losses[i];
        }
        avgGain /= period;
        avgLoss /= period;

        var rs = avgLoss == 0 ? 100 : avgGain / avgLoss;
        var rsi = 100 - (100 / (1 + rs));
        result.Add(new ChartPoint(period, rsi));

        for (var i = period + 1; i < data.Count; i++)
        {
            avgGain = (avgGain * (period - 1) + gains[i]) / period;
            avgLoss = (avgLoss * (period - 1) + losses[i]) / period;
            rs = avgLoss == 0 ? 100 : avgGain / avgLoss;
            rsi = 100 - (100 / (1 + rs));
            result.Add(new ChartPoint(i, rsi));
        }

        return result;
    }
}

/// <summary>
/// Composite financial chart model containing candlestick series, volume bars, and overlay indicators.
/// </summary>
public class FinancialChartModel
{
    public string Title { get; set; } = "Market Data";
    public CandlestickSeriesModel Candlesticks { get; set; } = new();
    public List<LineSeriesModel> Overlays { get; set; } = new();
    public bool ShowVolume { get; set; } = true;
    public ChartColor BullishColor { get; set; } = ChartColor.FromHex("#10B981");
    public ChartColor BearishColor { get; set; } = ChartColor.FromHex("#EF4444");

    public void AddSmaOverlay(int period, ChartColor color)
    {
        var smaPoints = TechnicalIndicators.CalculateSma(Candlesticks.Data, period);
        var line = new LineSeriesModel
        {
            Title = $"SMA ({period})",
            Color = color,
            Points = smaPoints.ToList(),
            StrokeWidth = 1.5f
        };
        Overlays.Add(line);
    }
}
