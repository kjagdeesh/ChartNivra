namespace ChartNivra.Core.Charts;

using ChartNivra.Core.Axes;
using ChartNivra.Core.Series;
using ChartNivra.Core.Themes;

/// <summary>
/// Model for grouped and stacked bar / column charts. Series points use X = category index, Y = value.
/// </summary>
public class BarChartModel
{
    public string Title { get; set; } = string.Empty;
    public ChartTheme Theme { get; set; } = ChartTheme.Light;
    public List<string> Categories { get; } = new();
    public List<BarSeriesModel> Series { get; } = new();

    /// <summary>When true series are stacked on top of each other; otherwise they are grouped side by side.</summary>
    public bool IsStacked { get; set; }

    public bool ShowLegend { get; set; } = true;
    public bool ShowValueLabels { get; set; }

    /// <summary>Computes value range for Y axis.</summary>
    public (double niceMinY, double niceMaxY, double stepY) ComputeYRange()
    {
        double max = 0;
        for (var c = 0; c < Categories.Count; c++)
        {
            double sum = 0;
            foreach (var s in Series)
            {
                var v = s.Points.FirstOrDefault(p => (int)p.X == c).Y;
                if (IsStacked) sum += Math.Max(0, v); else max = Math.Max(max, v);
            }
            if (IsStacked) max = Math.Max(max, sum);
        }
        return NiceScaleCalculator.Calculate(0, max <= 0 ? 1 : max, 5);
    }
}
