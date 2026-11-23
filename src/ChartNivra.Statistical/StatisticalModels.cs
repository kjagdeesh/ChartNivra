namespace ChartNivra.Statistical;

using ChartNivra.Core.Axes;
using ChartNivra.Core.Primitives;
using ChartNivra.Core.Series;
using ChartNivra.Core.Themes;

/// <summary>
/// Data item for five-number summary Box Plot.
/// </summary>
public readonly record struct BoxPlotPoint(string Category, double Min, double Q1, double Median, double Q3, double Max);

/// <summary>
/// Box and Whisker statistical chart series model.
/// </summary>
public class BoxPlotSeriesModel : SeriesModelBase
{
    public List<BoxPlotPoint> Data { get; set; } = new();

    public override DataRange GetXRange() => new(0, Math.Max(1, Data.Count - 1));

    public override DataRange GetYRange()
    {
        if (Data.Count == 0) return new DataRange(0, 1);
        double min = double.MaxValue, max = double.MinValue;
        foreach (var p in Data)
        {
            if (p.Min < min) min = p.Min;
            if (p.Max > max) max = p.Max;
        }
        return new DataRange(min, max);
    }
}

/// <summary>
/// Histogram chart model for frequency distribution visualization.
/// </summary>
public class HistogramModel
{
    public string Title { get; set; } = "Histogram";
    public int BinCount { get; set; } = 20;
    public ChartColor BarColor { get; set; } = ChartColor.FromHex("#00C096");
    public float BarOpacity { get; set; } = 0.8f;
    public List<double> Data { get; set; } = new();
}

/// <summary>
/// Histogram binning calculation helper.
/// </summary>
public static class HistogramCalculator
{
    public static (double binWidth, int[] bins, double min, double max) ComputeBins(IReadOnlyList<double> values, int binCount)
    {
        if (values == null || values.Count == 0 || binCount <= 0)
            return (0, Array.Empty<int>(), 0, 0);

        var min = values.Min();
        var max = values.Max();
        if (Math.Abs(max - min) < 1e-9) max = min + 1;

        var binWidth = (max - min) / binCount;
        var bins = new int[binCount];

        foreach (var v in values)
        {
            var idx = (int)Math.Floor((v - min) / binWidth);
            if (idx >= binCount) idx = binCount - 1;
            if (idx < 0) idx = 0;
            bins[idx]++;
        }

        return (binWidth, bins, min, max);
    }
}
