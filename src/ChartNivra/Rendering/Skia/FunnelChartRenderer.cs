namespace ChartNivra.Rendering.Skia;

using ChartNivra.Core.Primitives;
using ChartNivra.Core.Themes;
using ChartNivra.Cartesian;
using ChartNivra.Rendering.Abstractions;

public class FunnelChartRenderer : IChartRenderer
{
    public ChartTheme Theme { get; set; } = ChartTheme.Light;
    private readonly FunnelSeriesModel _model;

    public FunnelChartRenderer(FunnelSeriesModel model)
    {
        _model = model;
    }

    public void Render(IChartCanvas canvas, ChartRect bounds)
    {
        var theme = Theme;
        canvas.DrawRectangle(bounds, new ChartBrush(theme.BackgroundColor));

        var stages = _model.Stages;
        if (stages.Count == 0) return;
        var maxValue = stages.Max(s => Math.Max(0, s.Value));
        if (maxValue <= 0 || bounds.Width < 100 || bounds.Height < 100) return;

        canvas.DrawText(_model.Title, new ChartPoint(bounds.Left + 20, bounds.Top + 30), theme.TextColor, 16f, isBold: true);
        var plotArea = new ChartRect(bounds.Left + 20, bounds.Top + 55, bounds.Width - 40, bounds.Height - 75);
        var compact = plotArea.Width < 360;
        var funnelWidth = plotArea.Width * (compact ? .54 : .58);
        var centerX = plotArea.Left + funnelWidth / 2;
        var labelX = plotArea.Left + funnelWidth + 14;
        var stageHeight = plotArea.Height / stages.Count;
        var gap = Math.Min(5, stageHeight * .12);
        var colors = new[] { "#0284C7", "#2563EB", "#6366F1", "#8B5CF6", "#0D9488" };

        for (int i = 0; i < stages.Count; i++)
        {
            var stage = stages[i];
            var width = funnelWidth * Math.Clamp(stage.Value / maxValue, 0, 1);
            var nextRatio = i + 1 < stages.Count
                ? Math.Clamp(stages[i + 1].Value / maxValue, 0, 1)
                : Math.Clamp(stage.Value / maxValue * Math.Clamp(_model.NeckWidthRatio, 0, 1), 0, 1);
            var nextWidth = funnelWidth * nextRatio;
            var top = plotArea.Top + i * stageHeight;
            var height = Math.Max(1, stageHeight - gap);
            var color = stage.Color ?? ChartColor.FromHex(colors[i % colors.Length]);

            var points = new List<ChartPoint>
            {
                new(centerX - width / 2, top),
                new(centerX + width / 2, top),
                new(centerX + nextWidth / 2, top + height),
                new(centerX - nextWidth / 2, top + height)
            };
            points.Add(points[0]);

            canvas.DrawPolygon(points, new ChartBrush(color), new ChartPen(theme.BackgroundColor, 2f));

            var middleY = top + height / 2;
            var edgeX = centerX + (width + nextWidth) / 4;
            canvas.DrawLine(new ChartPoint(edgeX + 3, middleY), new ChartPoint(labelX - 6, middleY), new ChartPen(theme.AxisLineColor, 1f));
            canvas.DrawCircle(new ChartPoint(labelX, middleY - 9), 3, new ChartBrush(color));
            canvas.DrawText(stage.Name, new ChartPoint(labelX + 8, middleY - 5), theme.TextColor, compact ? 10f : 12f, true);
            var conversion = stage.Value / maxValue * 100;
            var detail = $"{stage.Value:N0}  ·  {conversion:0.#}%";
            canvas.DrawText(detail, new ChartPoint(labelX + 8, middleY + 12), theme.SecondaryTextColor, compact ? 9f : 11f);
        }
    }
}
