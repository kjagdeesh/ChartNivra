namespace ChartNivra.Core.Series;

using ChartNivra.Core.Axes;
using ChartNivra.Core.Primitives;
using ChartNivra.Core.Transforms;
using ChartNivra.Rendering.Abstractions;

/// <summary>
/// Basic line series representing a continuous line plot.
/// </summary>
public class LineSeriesModel
{
    public string Title { get; set; } = string.Empty;
    public ChartColor Color { get; set; } = ChartColor.FromHex("#3B82F6");
    public float StrokeWidth { get; set; } = 2.5f;
    public bool ShowMarkers { get; set; } = true;
    public float MarkerRadius { get; set; } = 4f;

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

    public void Render(IChartCanvas canvas, CartesianTransform transform)
    {
        if (Points.Count == 0) return;

        var screenPoints = new List<ChartPoint>(Points.Count);
        foreach (var pt in Points)
        {
            screenPoints.Add(transform.DataToPixel(pt.X, pt.Y));
        }

        var pen = new ChartPen(Color, StrokeWidth);
        canvas.DrawLines(screenPoints, pen);

        if (ShowMarkers)
        {
            var brush = new ChartBrush(Color);
            var markerBorder = new ChartPen(ChartColor.White, 1.5f);
            foreach (var pt in screenPoints)
            {
                canvas.DrawCircle(pt, MarkerRadius, brush, markerBorder);
            }
        }
    }
}
