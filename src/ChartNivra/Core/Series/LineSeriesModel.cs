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

    /// <summary>Shows the data point's value label directly on the chart when space permits.</summary>
    public bool ShowDataLabels { get; set; }

    /// <summary>Optional numeric format used when a rich data point has no explicit <see cref="ChartDataPoint.ValueLabel"/>.</summary>
    public string DataLabelFormat { get; set; } = "G4";

    public List<ChartPoint> Points { get; set; } = new();

    /// <summary>
    /// Rich data input for labels, tooltips, colors and domain metadata. When populated it takes
    /// precedence over <see cref="Points"/>, keeping the simple X/Y API fully backwards compatible.
    /// </summary>
    public List<ChartDataPoint> DataPoints { get; } = new();

    public IReadOnlyList<ChartPoint> GetPoints() => DataPoints.Count == 0
        ? Points
        : DataPoints.Select(point => point.ToChartPoint()).ToArray();

    public DataRange GetXRange()
    {
        var points = GetPoints();
        if (points.Count == 0) return new DataRange(0, 1);
        double min = double.MaxValue, max = double.MinValue;
        foreach (var pt in points)
        {
            if (pt.X < min) min = pt.X;
            if (pt.X > max) max = pt.X;
        }
        return new DataRange(min, max);
    }

    public DataRange GetYRange()
    {
        var points = GetPoints();
        if (points.Count == 0) return new DataRange(0, 1);
        double min = double.MaxValue, max = double.MinValue;
        foreach (var pt in points)
        {
            if (pt.Y < min) min = pt.Y;
            if (pt.Y > max) max = pt.Y;
        }
        return new DataRange(min, max);
    }
}

