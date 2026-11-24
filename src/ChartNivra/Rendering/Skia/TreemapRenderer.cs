namespace ChartNivra.Rendering.Skia;

using ChartNivra.Core.Primitives;
using ChartNivra.Core.Themes;
using ChartNivra.Pie;
using ChartNivra.Rendering.Abstractions;

public class TreemapRenderer : IChartRenderer
{
    private readonly TreemapModel _model;

    public TreemapRenderer(TreemapModel model)
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

        var plotArea = new ChartRect(bounds.Left + 20, bounds.Top + topMargin, bounds.Width - 40, bounds.Height - topMargin - 20);

        var root = _model.Root;
        root.ComputeTotalValue();

        DrawTreemap(canvas, root, plotArea, theme);
    }

    private void DrawTreemap(IChartCanvas canvas, HierarchyNode node, ChartRect rect, ChartTheme theme)
    {
        if (node.Children.Count == 0)
        {
            var color = node.Color ?? theme.DefaultPalette.GetColor(0);
            var drawRect = new ChartRect(
                rect.Left + _model.BorderSpacing / 2,
                rect.Top + _model.BorderSpacing / 2,
                rect.Width - _model.BorderSpacing,
                rect.Height - _model.BorderSpacing);

            canvas.DrawRectangle(drawRect, new ChartBrush(color), new ChartPen(theme.BackgroundColor, (float)_model.BorderSpacing));

            if (!string.IsNullOrEmpty(node.Name) && drawRect.Width > 40 && drawRect.Height > 20)
            {
                var labelSize = canvas.MeasureText(node.Name, 10f, true);
                if (labelSize.Width < drawRect.Width - 10 && drawRect.Height > 32)
                {
                    canvas.DrawText(node.Name, new ChartPoint(drawRect.Left + 6, drawRect.Top + 15), ChartColor.White, 10f, true);
                    canvas.DrawText(node.Value.ToString("N0"), new ChartPoint(drawRect.Left + 6, drawRect.Top + 29), ChartColor.White.WithAlpha(220), 9f);
                }
            }
        }
        else
        {
            // Squarified layout - simplified: horizontal slices
            var total = node.Value;
            var x = rect.Left;
            var y = rect.Top;

            foreach (var child in node.Children)
            {
                var ratio = child.Value / total;
                var childWidth = rect.Width * ratio;
                var childRect = new ChartRect(x, y, childWidth, rect.Height);
                DrawTreemap(canvas, child, childRect, theme);
                x += childWidth;
            }
        }
    }
}
