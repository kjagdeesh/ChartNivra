namespace ChartNivra.Core.Financial;

using ChartNivra.Core.Axes;
using ChartNivra.Core.Primitives;
using ChartNivra.Core.Series;

/// <summary>
/// Financial data point representing Open, High, Low, Close, and Volume.
/// </summary>
public readonly record struct FinancialPoint(DateTime Date, double Open, double High, double Low, double Close, double Volume = 0)
{
    public bool IsBullish => Close >= Open;
}

/// <summary>
/// Series model for financial Candlestick and OHLC charts.
/// </summary>
public class CandlestickSeriesModel : SeriesModelBase
{
    public ChartColor BullishColor { get; set; } = ChartColor.FromHex("#10B981"); // Green
    public ChartColor BearishColor { get; set; } = ChartColor.FromHex("#EF4444"); // Red
    public float WickWidth { get; set; } = 1.5f;
    public float CandleWidthRatio { get; set; } = 0.7f;

    public List<FinancialPoint> Data { get; set; } = new();

    public override DataRange GetXRange()
    {
        if (Data.Count == 0) return new DataRange(0, 1);
        return new DataRange(0, Data.Count - 1);
    }

    public override DataRange GetYRange()
    {
        if (Data.Count == 0) return new DataRange(0, 1);
        var min = double.MaxValue;
        var max = double.MinValue;
        foreach (var p in Data)
        {
            if (p.Low < min) min = p.Low;
            if (p.High > max) max = p.High;
        }
        return new DataRange(min, max);
    }
}

/// <summary>
/// Computes technical indicators such as Simple Moving Average (SMA) and Exponential Moving Average (EMA).
/// </summary>
public static class TechnicalIndicators
{
    public static IReadOnlyList<ChartPoint> CalculateSma(IReadOnlyList<FinancialPoint> data, int period)
    {
        if (data == null || data.Count < period || period <= 0)
            return Array.Empty<ChartPoint>();

        var result = new List<ChartPoint>(data.Count - period + 1);
        var sum = 0.0;

        for (var i = 0; i < period; i++)
        {
            sum += data[i].Close;
        }
        result.Add(new ChartPoint(period - 1, sum / period));

        for (var i = period; i < data.Count; i++)
        {
            sum += data[i].Close - data[i - period].Close;
            result.Add(new ChartPoint(i, sum / period));
        }

        return result;
    }

    public static IReadOnlyList<ChartPoint> CalculateEma(IReadOnlyList<FinancialPoint> data, int period)
    {
        if (data == null || data.Count < period || period <= 0)
            return Array.Empty<ChartPoint>();

        var result = new List<ChartPoint>(data.Count);
        var multiplier = 2.0 / (period + 1.0);

        // Seed with SMA
        var sum = 0.0;
        for (var i = 0; i < period; i++)
        {
            sum += data[i].Close;
        }
        var prevEma = sum / period;
        result.Add(new ChartPoint(period - 1, prevEma));

        for (var i = period; i < data.Count; i++)
        {
            var ema = (data[i].Close - prevEma) * multiplier + prevEma;
            result.Add(new ChartPoint(i, ema));
            prevEma = ema;
        }

        return result;
    }
}
