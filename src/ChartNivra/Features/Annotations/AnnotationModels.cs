namespace ChartNivra.Annotations;

using ChartNivra.Core.Primitives;
using ChartNivra.Core.Transforms;
using ChartNivra.Rendering.Abstractions;

/// <summary>
/// Base class for all chart annotations.
/// </summary>
public abstract class AnnotationBase
{
    public ChartColor Color { get; set; } = ChartColor.FromHex("#F59E0B");
    public abstract void Render(IChartCanvas canvas, CartesianTransform transform);
}

/// <summary>
/// Horizontal or vertical reference line annotation (e.g. Target, Threshold, Average).
/// </summary>
public class ReferenceLineAnnotation : AnnotationBase
{
    public double Value { get; set; }
    public bool IsHorizontal { get; set; } = true;
    public float StrokeWidth { get; set; } = 1.5f;
    public string Label { get; set; } = string.Empty;

    public override void Render(IChartCanvas canvas, CartesianTransform transform)
    {
        var pen = new ChartPen(Color, StrokeWidth, IsDashed: true);
        if (IsHorizontal)
        {
            var p1 = transform.DataToPixel(transform.XRange.Min, Value);
            var p2 = transform.DataToPixel(transform.XRange.Max, Value);
            canvas.DrawLine(p1, p2, pen);

            if (!string.IsNullOrEmpty(Label))
            {
                canvas.DrawText(Label, new ChartPoint(p1.X + 5, p1.Y - 5), Color, 11f);
            }
        }
    }
}
