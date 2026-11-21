namespace ChartNivra.Rendering.Abstractions;

using ChartNivra.Core.Primitives;

/// <summary>
/// Stroke/Pen definition.
/// </summary>
public record ChartPen(ChartColor Color, float StrokeWidth = 1f, bool IsDashed = false);

/// <summary>
/// Fill/Brush definition.
/// </summary>
public record ChartBrush(ChartColor Color);

/// <summary>
/// Hardware/engine agnostic 2D drawing canvas abstraction.
/// </summary>
public interface IChartCanvas : IDisposable
{
    void Clear(ChartColor color);
    void DrawLine(ChartPoint start, ChartPoint end, ChartPen pen);
    void DrawLines(IReadOnlyList<ChartPoint> points, ChartPen pen);
    void DrawRectangle(ChartRect rect, ChartBrush? brush, ChartPen? pen = null);
    void DrawCircle(ChartPoint center, float radius, ChartBrush? brush, ChartPen? pen = null);
    void DrawText(string text, ChartPoint position, ChartColor color, float fontSize, bool isBold = false);
    ChartSize MeasureText(string text, float fontSize, bool isBold = false);
    void Save();
    void Restore();
    void ClipRect(ChartRect rect);
}

/// <summary>
/// Renderer contract for charts.
/// </summary>
public interface IChartRenderer
{
    void Render(IChartCanvas canvas, ChartRect bounds);
}
