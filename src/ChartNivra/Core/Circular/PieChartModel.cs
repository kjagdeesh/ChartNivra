namespace ChartNivra.Core.Circular;

using ChartNivra.Core.Primitives;
using ChartNivra.Core.Themes;

/// <summary>
/// Data item for Pie and Donut slices.
/// </summary>
public class PieSliceModel
{
    public string Label { get; set; } = string.Empty;
    public double Value { get; set; }
    public ChartColor Color { get; set; } = ChartColor.Transparent;
    public bool IsExploded { get; set; } = false;
}

/// <summary>
/// Model for Pie and Donut circular charts.
/// </summary>
public class PieChartModel
{
    public string Title { get; set; } = string.Empty;
    public ChartTheme Theme { get; set; } = ChartTheme.Dark;
    public List<PieSliceModel> Slices { get; } = new();

    /// <summary>
    /// Inner radius ratio: 0.0 for Pie chart, 0.4 - 0.7 for Donut chart.
    /// </summary>
    public float HoleRadiusRatio { get; set; } = 0.0f;

    public double TotalValue => Slices.Sum(s => Math.Max(0, s.Value));
}
