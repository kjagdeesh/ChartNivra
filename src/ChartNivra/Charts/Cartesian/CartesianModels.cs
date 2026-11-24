namespace ChartNivra.Cartesian;

using ChartNivra.Core.Axes;
using ChartNivra.Core.Primitives;
using ChartNivra.Core.Series;

/// <summary>
/// Spline (cardinal/catmull-rom) interpolated curve series model.
/// </summary>
public class SplineSeriesModel : SeriesModelBase
{
    public double Tension { get; set; } = 0.5;
    public float StrokeWidth { get; set; } = 2.5f;
    public List<ChartPoint> Points { get; set; } = new();

    public SplineSeriesModel(string title, ChartColor color)
    {
        Title = title;
        Color = color;
    }

    public override DataRange GetXRange()
    {
        if (Points.Count == 0) return new DataRange(0, 1);
        double min = double.MaxValue, max = double.MinValue;
        foreach (var pt in Points)
        {
            if (pt.X < min) min = pt.X;
            if (pt.X > max) max = pt.X;
        }
        return new DataRange(min, max);
    }

    public override DataRange GetYRange()
    {
        if (Points.Count == 0) return new DataRange(0, 1);
        double min = double.MaxValue, max = double.MinValue;
        foreach (var pt in Points)
        {
            if (pt.Y < min) min = pt.Y;
            if (pt.Y > max) max = pt.Y;
        }
        return new DataRange(min, max);
    }
}

/// <summary>
/// Step line series model (step-before, step-after, or step-center).
/// </summary>
public enum StepMode { Before, Center, After }

public class StepLineSeriesModel : SeriesModelBase
{
    public StepMode Mode { get; set; } = StepMode.After;
    public float StrokeWidth { get; set; } = 2.0f;
    public List<ChartPoint> Points { get; set; } = new();

    public StepLineSeriesModel(string title, ChartColor color)
    {
        Title = title;
        Color = color;
    }

    public override DataRange GetXRange()
    {
        if (Points.Count == 0) return new DataRange(0, 1);
        double min = double.MaxValue, max = double.MinValue;
        foreach (var pt in Points)
        {
            if (pt.X < min) min = pt.X;
            if (pt.X > max) max = pt.X;
        }
        return new DataRange(min, max);
    }

    public override DataRange GetYRange()
    {
        if (Points.Count == 0) return new DataRange(0, 1);
        double min = double.MaxValue, max = double.MinValue;
        foreach (var pt in Points)
        {
            if (pt.Y < min) min = pt.Y;
            if (pt.Y > max) max = pt.Y;
        }
        return new DataRange(min, max);
    }
}

/// <summary>Single-value KPI displayed against qualitative bands and a target marker.</summary>
public class BulletChartModel
{
    public string Title { get; set; } = "Performance";
    public double Value { get; set; }
    public double Target { get; set; }
    public double Maximum { get; set; } = 100;
    public List<double> QualitativeRanges { get; set; } = new() { 45, 70, 100 };
    public ChartColor ValueColor { get; set; } = ChartColor.FromHex("#38BDF8");
    public ChartColor TargetColor { get; set; } = ChartColor.FromHex("#F97316");
}

/// <summary>
/// Column series model (vertical bars).
/// </summary>
public class ColumnSeriesModel : BarSeriesModel
{
    public ColumnSeriesModel(string title, ChartColor color)
    {
        Title = title;
        Color = color;
    }
}

/// <summary>
/// Bubble series data point with size metric.
/// </summary>
public record BubbleDataPoint(double X, double Y, double Size, string? Label = null);

/// <summary>
/// Bubble series model with 3-dimensional data (X, Y, Size).
/// </summary>
public class BubbleSeriesModel
{
    public string Name { get; set; }
    public ChartColor Color { get; set; }
    public List<BubbleDataPoint> Points { get; set; } = new();
    public double MinBubbleRadius { get; set; } = 4.0;
    public double MaxBubbleRadius { get; set; } = 30.0;

    public BubbleSeriesModel(string name, ChartColor color)
    {
        Name = name;
        Color = color;
    }
}

/// <summary>
/// Range data point with high and low values.
/// </summary>
public record RangeDataPoint(double X, double Low, double High, string? Label = null);

/// <summary>
/// Range Area or Range Column series model.
/// </summary>
public class RangeSeriesModel
{
    public string Name { get; set; }
    public ChartColor Color { get; set; }
    public double Opacity { get; set; } = 0.5;
    public List<RangeDataPoint> Points { get; set; } = new();

    public RangeSeriesModel(string name, ChartColor color)
    {
        Name = name;
        Color = color;
    }
}

/// <summary>
/// Waterfall chart slice type.
/// </summary>
public enum WaterfallType { Incremental, Decremental, Total }

public record WaterfallPoint(string Category, double Value, WaterfallType Type = WaterfallType.Incremental);

/// <summary>
/// Waterfall chart model for financial bridge analysis.
/// </summary>
public class WaterfallSeriesModel
{
    public string Name { get; set; }
    public ChartColor PositiveColor { get; set; } = ChartColor.FromHex("#10B981");
    public ChartColor NegativeColor { get; set; } = ChartColor.FromHex("#EF4444");
    public ChartColor TotalColor { get; set; } = ChartColor.FromHex("#3B82F6");
    public List<WaterfallPoint> Points { get; set; } = new();

    public WaterfallSeriesModel(string name)
    {
        Name = name;
    }
}

/// <summary>
/// Funnel/Pyramid chart item representing conversion stages.
/// </summary>
public record FunnelStage(string Name, double Value, ChartColor? Color = null);

/// <summary>
/// Funnel chart model for pipeline conversion visualization.
/// </summary>
public class FunnelSeriesModel
{
    public string Title { get; set; } = "Funnel Analysis";
    public List<FunnelStage> Stages { get; set; } = new();
    public double NeckWidthRatio { get; set; } = 0.3;
    public double NeckHeightRatio { get; set; } = 0.3;
}
