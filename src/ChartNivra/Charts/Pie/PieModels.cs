namespace ChartNivra.Pie;

using ChartNivra.Core.Circular;
using ChartNivra.Core.Primitives;

/// <summary>
/// Donut chart model with inner cut-out radius and optional center text/badge.
/// </summary>
public class DonutChartModel : PieChartModel
{
    public string? CenterTitle { get; set; }
    public string? CenterSubtitle { get; set; }
    public ChartColor CenterTextColor { get; set; } = ChartColor.FromHex("#0F172A");

    public DonutChartModel()
    {
        HoleRadiusRatio = 0.6f;
    }

    public DonutChartModel(string title) : this()
    {
        Title = title;
    }
}

/// <summary>
/// Hierarchical node for Sunburst, Treemap, and Pack charts.
/// </summary>
public class HierarchyNode
{
    public string Name { get; set; }
    public double Value { get; set; }
    public ChartColor? Color { get; set; }
    public List<HierarchyNode> Children { get; set; } = new();

    public HierarchyNode(string name, double value = 0, ChartColor? color = null)
    {
        Name = name;
        Value = value;
        Color = color;
    }

    public double ComputeTotalValue()
    {
        if (Children.Count == 0) return Value;
        var sum = 0.0;
        foreach (var child in Children)
        {
            sum += child.ComputeTotalValue();
        }
        Value = sum;
        return sum;
    }
}

/// <summary>
/// Sunburst multi-level radial chart model.
/// </summary>
public class SunburstModel
{
    public string Title { get; set; }
    public HierarchyNode Root { get; set; }
    public double InnerRadiusRatio { get; set; } = 0.2;

    public SunburstModel(string title, HierarchyNode root)
    {
        Title = title;
        Root = root;
    }
}

/// <summary>
/// Squarified / slice-and-dice Treemap chart model.
/// </summary>
public class TreemapModel
{
    public string Title { get; set; }
    public HierarchyNode Root { get; set; }
    public double BorderSpacing { get; set; } = 2.0;

    public TreemapModel(string title, HierarchyNode root)
    {
        Title = title;
        Root = root;
    }
}
