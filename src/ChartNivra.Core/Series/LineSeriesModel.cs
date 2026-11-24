namespace ChartNivra.Core.Series;

using ChartNivra.Core.Axes;
using ChartNivra.Core.Primitives;
using ChartNivra.Core.Transforms;

/// <summary>
/// Basic line series representing a continuous line plot data model.
/// </summary>
public class LineSeriesModel
{
    public string Title { get; set; } = string.Empty;
    public ChartColor Color { get; set; } = ChartColor.FromHex("#3B82F6");
    public float StrokeWidth { get; set; } = 2.5f;
    public bool ShowMarkers { get; set; } = true;
    public float MarkerRadius { get; set; } = 4f;

    /// <summary>Renders the line as a smooth Catmull-Rom spline instead of straight segments.</summary>
    public bool Smooth { get; set; }

    /// <summary>Fills the region between the line and the X axis.</summary>
    public bool ShowArea { get; set; }

    /// <summary>Alpha (0-255) of the area fill.</summary>
    public byte AreaOpacity { get; set; } = 60;

    public List<ChartPoint> Points { get; set; } = new();

    public DataRange GetXRange()
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

    public DataRange GetYRange()
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

