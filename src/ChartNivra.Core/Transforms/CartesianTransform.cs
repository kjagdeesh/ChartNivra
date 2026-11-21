namespace ChartNivra.Core.Transforms;

using ChartNivra.Core.Axes;
using ChartNivra.Core.Primitives;

/// <summary>
/// Transforms data coordinates (X, Y) into screen canvas pixels and vice-versa.
/// </summary>
public class CartesianTransform
{
    public DataRange XRange { get; set; }
    public DataRange YRange { get; set; }
    public ChartRect PlotArea { get; set; }

    public CartesianTransform(DataRange xRange, DataRange yRange, ChartRect plotArea)
    {
        XRange = xRange;
        YRange = yRange;
        PlotArea = plotArea;
    }

    public ChartPoint DataToPixel(double dataX, double dataY)
    {
        if (XRange.Delta == 0 || YRange.Delta == 0)
            return ChartPoint.Zero;

        var normX = (dataX - XRange.Min) / XRange.Delta;
        var normY = (dataY - YRange.Min) / YRange.Delta;

        var px = PlotArea.Left + normX * PlotArea.Width;
        var py = PlotArea.Bottom - normY * PlotArea.Height; // Invert Y for screen coordinates

        return new ChartPoint(px, py);
    }

    public ChartPoint PixelToData(double px, double py)
    {
        if (PlotArea.Width == 0 || PlotArea.Height == 0)
            return ChartPoint.Zero;

        var normX = (px - PlotArea.Left) / PlotArea.Width;
        var normY = (PlotArea.Bottom - py) / PlotArea.Height;

        var dataX = XRange.Min + normX * XRange.Delta;
        var dataY = YRange.Min + normY * YRange.Delta;

        return new ChartPoint(dataX, dataY);
    }
}
