namespace ChartNivra.Rendering.Skia;

using ChartNivra.Core.Primitives;
using ChartNivra.Core.Themes;
using ChartNivra.Pie;
using ChartNivra.Rendering.Abstractions;

public class SunburstRenderer : IChartRenderer
{
    public ChartTheme Theme { get; set; } = ChartTheme.Light;
    private readonly SunburstModel _model;

    public SunburstRenderer(SunburstModel model)
    {
        _model = model;
    }

    public void Render(IChartCanvas canvas, ChartRect bounds)
    {
        var theme = Theme;
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

        if (_model.Root.ComputeTotalValue() <= 0 || _model.Root.Children.Count == 0) return;
        var innerRadius = _model.InnerRadiusRatio * outerRadius;
        var levels = Math.Max(1, GetDepth(_model.Root));
        var ringWidth = (outerRadius - innerRadius) / levels;
        DrawChildren(canvas, _model.Root, centerX, centerY, innerRadius, ringWidth, -Math.PI / 2, 2 * Math.PI, 0, theme);
    }

    private void DrawChildren(IChartCanvas canvas, HierarchyNode parent, double cx, double cy, double baseInnerRadius, double ringWidth, double startAngle, double sweepAngle, int depth, ChartTheme theme)
    {
        var total = parent.Children.Sum(child => child.ComputeTotalValue());
        if (total <= 0) return;
        var childStart = startAngle;
        for (var index = 0; index < parent.Children.Count; index++)
        {
            var child = parent.Children[index];
            var childSweep = sweepAngle * (child.ComputeTotalValue() / total);
            var innerR = baseInnerRadius + depth * ringWidth;
            var outerR = innerR + ringWidth;
            var color = child.Color ?? theme.DefaultPalette.GetColor(index + depth);
            var points = new List<ChartPoint>();
            var segments = Math.Max(3, (int)(childSweep / .08));
            for (var i = 0; i <= segments; i++)
            {
                var angle = childStart + childSweep * i / segments;
                points.Add(new ChartPoint(cx + outerR * Math.Cos(angle), cy + outerR * Math.Sin(angle)));
            }
            for (var i = segments; i >= 0; i--)
            {
                var angle = childStart + childSweep * i / segments;
                points.Add(new ChartPoint(cx + innerR * Math.Cos(angle), cy + innerR * Math.Sin(angle)));
            }
            canvas.DrawPolygon(points, new ChartBrush(color), new ChartPen(theme.BackgroundColor, 2f));
            if (childSweep > .38)
            {
                var mid = childStart + childSweep / 2;
                var labelRadius = (innerR + outerR) / 2;
                var size = canvas.MeasureText(child.Name, 9, true);
                canvas.DrawText(child.Name, new ChartPoint(cx + labelRadius * Math.Cos(mid) - size.Width / 2, cy + labelRadius * Math.Sin(mid) + size.Height / 2), ChartColor.White, 9, true);
            }
            if (child.Children.Count > 0)
                DrawChildren(canvas, child, cx, cy, baseInnerRadius, ringWidth, childStart, childSweep, depth + 1, theme);
            childStart += childSweep;
        }
    }

    private static int GetDepth(HierarchyNode node) => node.Children.Count == 0 ? 0 : 1 + node.Children.Max(GetDepth);
}
