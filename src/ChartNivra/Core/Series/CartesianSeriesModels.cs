namespace ChartNivra.Core.Series;

using ChartNivra.Core.Axes;
using ChartNivra.Core.Primitives;

/// <summary>
/// Base contract for all chart series models.
/// </summary>
public abstract class SeriesModelBase
{
    public string Title { get; set; } = string.Empty;
    public ChartColor Color { get; set; } = ChartColor.Transparent;
    public bool IsVisible { get; set; } = true;

    public abstract DataRange GetXRange();
    public abstract DataRange GetYRange();
}

/// <summary>
/// Cartesian series supporting Bar and Column rendering.
/// </summary>
public class BarSeriesModel : SeriesModelBase
{
    public float BarWidthRatio { get; set; } = 0.6f;
    public float CornerRadius { get; set; } = 4f;
    public List<ChartPoint> Points { get; set; } = new();

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
        double min = 0, max = double.MinValue;
        foreach (var pt in Points)
        {
            if (pt.Y < min) min = pt.Y;
            if (pt.Y > max) max = pt.Y;
        }
        return new DataRange(min, max);
    }
}

/// <summary>
/// Area series model for filled area plots beneath a line.
/// </summary>
public class AreaSeriesModel : SeriesModelBase
{
    public float StrokeWidth { get; set; } = 2f;
    public byte FillOpacity { get; set; } = 100;
    public List<ChartPoint> Points { get; set; } = new();

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
        double min = 0, max = double.MinValue;
        foreach (var pt in Points)
        {
            if (pt.Y < min) min = pt.Y;
            if (pt.Y > max) max = pt.Y;
        }
        return new DataRange(min, max);
    }
}

/// <summary>
/// Scatter and bubble series model.
/// </summary>
public class ScatterSeriesModel : SeriesModelBase
{
    public float DefaultPointRadius { get; set; } = 5f;
    public List<ChartPoint> Points { get; set; } = new();

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
