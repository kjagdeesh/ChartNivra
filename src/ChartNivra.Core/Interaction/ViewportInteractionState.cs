namespace ChartNivra.Core.Interaction;

using ChartNivra.Core.Primitives;

/// <summary>
/// Tracks zoom and pan state for a chart viewport.
/// </summary>
public class ViewportInteractionState
{
    public double ZoomFactorX { get; set; } = 1.0;
    public double ZoomFactorY { get; set; } = 1.0;

    public double PanOffsetX { get; set; } = 0.0;
    public double PanOffsetY { get; set; } = 0.0;

    public ChartPoint? HoverPoint { get; set; }
    public ChartPoint? SelectedPoint { get; set; }

    public bool IsZoomEnabled { get; set; } = true;
    public bool IsPanEnabled { get; set; } = true;
    public bool IsCrosshairEnabled { get; set; } = true;

    public void Zoom(double scaleDelta, ChartPoint focalPoint)
    {
        if (!IsZoomEnabled) return;
        var nextZoomX = Math.Clamp(ZoomFactorX * scaleDelta, 1.0, 100.0);
        var nextZoomY = Math.Clamp(ZoomFactorY * scaleDelta, 1.0, 100.0);

        ZoomFactorX = nextZoomX;
        ZoomFactorY = nextZoomY;
    }

    public void Pan(double deltaX, double deltaY)
    {
        if (!IsPanEnabled) return;
        PanOffsetX += deltaX;
        PanOffsetY += deltaY;
    }

    public void Reset()
    {
        ZoomFactorX = 1.0;
        ZoomFactorY = 1.0;
        PanOffsetX = 0.0;
        PanOffsetY = 0.0;
        HoverPoint = null;
        SelectedPoint = null;
    }
}
