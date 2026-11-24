namespace ChartNivra.Rendering.Skia;

using ChartNivra.Core.Primitives;
using ChartNivra.Core.Themes;
using ChartNivra.Pie;
using ChartNivra.Rendering.Abstractions;

public class SunburstRenderer : IChartRenderer
{
    private readonly SunburstModel _model;

    public SunburstRenderer(SunburstModel model)
    {
        _model = model;
    }

    public void Render(IChartCanvas canvas, ChartRect bounds)
    {
        var theme = ChartTheme.Light;
        canvas.DrawRectangle(bounds, new ChartBrush(theme.BackgroundColor));

        var topMargin = 20.0;
        if (!string.IsNullOrEmpty(_model.Title))
        {
            canvas.DrawText(_model.Title, new ChartPoint(bounds.Left + 20, bounds.Top + 30), theme.TextColor, 16f, isBold: true);
            topMargin = 50.0;
        }

        var centerX = bounds.Left + bounds.Width / 2;
        var centerY = bounds.Top + topMargin + (bounds.Height - topMargin) / 2;
        var outerRadius = Math.Min(bounds.Width, bounds.Height - topMargin) * 0.35;

        DrawNode(canvas, _model.Root, centerX, centerY, _model.InnerRadiusRatio * outerRadius, outerRadius, -Math.PI / 2, 2 * Math.PI, theme);
    }

    private void DrawNode(IChartCanvas canvas, HierarchyNode node, double cx, double cy, double innerR, double outerR, double startAngle, double sweepAngle, ChartTheme theme)
    {
        if (node.Value <= 0) return;

        var totalValue = node.ComputeTotalValue();
        var nodeSweep = sweepAngle * (node.Value / totalValue);

        var color = node.Color ?? theme.DefaultPalette.GetColor(0);

        // Draw arc segment
        var points = new List<ChartPoint>();
        var segments = Math.Max(1, (int)(nodeSweep / 0.1));
        for (int i = 0; i <= segments; i++)
        {
            var angle = startAngle + nodeSweep * i / segments;
            points.Add(new ChartPoint(cx + outerR * Math.Cos(angle), cy + outerR * Math.Sin(angle)));
        }
        for (int i = segments; i >= 0; i--)
        {
            var angle = startAngle + nodeSweep * i / segments;
            points.Add(new ChartPoint(cx + innerR * Math.Cos(angle), cy + innerR * Math.Sin(angle)));
        }
        points.Add(points[0]);

        canvas.DrawLines(points, new ChartPen(color, 1f));

        // Draw label at middle of arc
        if (!string.IsNullOrEmpty(node.Name))
        {
            var midAngle = startAngle + nodeSweep / 2;
            var labelR = (innerR + outerR) / 2;
            var lx = cx + labelR * Math.Cos(midAngle);
            var ly = cy + labelR * Math.Sin(midAngle);
            var labelSize = canvas.MeasureText(node.Name, 10f);
            canvas.DrawText(node.Name, new ChartPoint(lx - labelSize.Width / 2, ly + labelSize.Height / 2), theme.TextColor, 10f);
        }

        // Draw children
        if (node.Children.Count > 0)
        {
            var childInnerR = outerR;
            var childOuterR = outerR + (outerR - innerR) * 0.8;
            var childStart = startAngle;

            foreach (var child in node.Children)
            {
                var childSweep = nodeSweep * (child.Value / node.Value);
                DrawNode(canvas, child, cx, cy, childInnerR, childOuterR, childStart, childSweep, theme);
                childStart += childSweep;
            }
        }
    }
}
