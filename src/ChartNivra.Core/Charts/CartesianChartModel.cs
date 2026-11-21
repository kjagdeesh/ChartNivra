namespace ChartNivra.Core.Charts;

using ChartNivra.Core.Axes;
using ChartNivra.Core.Primitives;
using ChartNivra.Core.Series;
using ChartNivra.Core.Themes;
using ChartNivra.Core.Transforms;

/// <summary>
/// Pure data and layout model for a Cartesian 2D chart.
/// </summary>
public class CartesianChartModel
{
    public string Title { get; set; } = string.Empty;
    public ChartTheme Theme { get; set; } = ChartTheme.Dark;
    public List<LineSeriesModel> Series { get; } = new();

    public (DataRange xRange, DataRange yRange, double niceMinY, double niceMaxY, double stepY) ComputeRanges()
    {
        if (Series.Count == 0)
        {
            return (new DataRange(0, 1), new DataRange(0, 1), 0, 1, 0.2);
        }

        var minX = double.MaxValue; var maxX = double.MinValue;
        var minY = double.MaxValue; var maxY = double.MinValue;

        foreach (var s in Series)
        {
            var xr = s.GetXRange();
            var yr = s.GetYRange();
            if (xr.Min < minX) minX = xr.Min;
            if (xr.Max > maxX) maxX = xr.Max;
            if (yr.Min < minY) minY = yr.Min;
            if (yr.Max > maxY) maxY = yr.Max;
        }

        var (niceMinY, niceMaxY, stepY) = NiceScaleCalculator.Calculate(minY, maxY, 5);
        return (new DataRange(minX, maxX), new DataRange(niceMinY, niceMaxY), niceMinY, niceMaxY, stepY);
    }
}

