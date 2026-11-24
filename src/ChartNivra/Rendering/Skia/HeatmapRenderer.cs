namespace ChartNivra.Rendering.Skia;

using ChartNivra.Core.Primitives;
using ChartNivra.Core.Themes;
using ChartNivra.Statistical;
using ChartNivra.Rendering.Abstractions;

public class HeatmapRenderer : IChartRenderer
{
    private readonly HeatmapModel _model;

    public HeatmapRenderer(HeatmapModel model)
    {
        _model = model;
    }

    public void Render(IChartCanvas canvas, ChartRect bounds)
    {
        var theme = _model.Theme ?? ChartTheme.Light;
        canvas.DrawRectangle(bounds, new ChartBrush(theme.BackgroundColor));
        var top = ChartDrawing.DrawTitle(canvas, _model.Title, bounds, theme);

        if (_model.Rows <= 0 || _model.Columns <= 0) return;

        if (_model.Mode == HeatmapMode.Calendar)
        {
            DrawCalendar(canvas, bounds, top, theme);
            return;
        }

        var plotArea = new ChartRect(bounds.Left + 60, bounds.Top + top, bounds.Width - 80, bounds.Height - top - 40);

        var cellWidth = plotArea.Width / _model.Columns;
        var cellHeight = plotArea.Height / _model.Rows;
        var valueRange = _model.MaxValue - _model.MinValue;
        if (valueRange < 1e-9) valueRange = 1;

        foreach (var cell in _model.Cells)
        {
            var x = plotArea.Left + cell.Column * cellWidth;
            var y = plotArea.Top + cell.Row * cellHeight;

            var t = (cell.Value - _model.MinValue) / valueRange;
            t = Math.Clamp(t, 0, 1);
            var color = LerpColor(_model.MinColor, _model.MaxColor, (float)t);

            canvas.DrawRectangle(new ChartRect(x, y, cellWidth - 1, cellHeight - 1), new ChartBrush(color));
        }
    }

    private void DrawCalendar(IChartCanvas canvas, ChartRect bounds, double top, ChartTheme theme)
    {
        var left = bounds.Left + 44;
        var availableWidth = bounds.Width - 62;
        var availableHeight = bounds.Height - top - 52;
        var cell = Math.Min(availableHeight / _model.Rows, availableWidth / _model.Columns);
        cell = Math.Max(5, cell);
        var gridWidth = cell * _model.Columns;
        var gridTop = bounds.Top + top + 14;
        var gridLeft = left + Math.Max(0, (availableWidth - gridWidth) / 2);
        var values = _model.Cells.ToDictionary(cell => (cell.Row, cell.Column), cell => cell);

        for (var row = 0; row < _model.Rows; row++)
        {
            if (_model.RowLabels.Count > row && (row == 1 || row == 3 || row == 5))
                canvas.DrawText(_model.RowLabels[row], new ChartPoint(gridLeft - 34, gridTop + row * cell + cell - 2), theme.SecondaryTextColor, 9);
            for (var column = 0; column < _model.Columns; column++)
            {
                var value = values.TryGetValue((row, column), out var datum) ? datum.Value : 0;
                var t = Math.Clamp((value - _model.MinValue) / Math.Max(.0001, _model.MaxValue - _model.MinValue), 0, 1);
                var color = value <= _model.MinValue ? ChartColor.FromHex("#EBEDF0") : LerpColor(_model.MinColor, _model.MaxColor, (float)t);
                var rect = new ChartRect(gridLeft + column * cell + 1, gridTop + row * cell + 1, Math.Max(2, cell - 3), Math.Max(2, cell - 3));
                canvas.DrawRoundRect(rect, 2, new ChartBrush(color));
            }
        }

        for (var column = 0; column < _model.ColumnLabels.Count && column < _model.Columns; column++)
        {
            if (string.IsNullOrWhiteSpace(_model.ColumnLabels[column])) continue;
            canvas.DrawText(_model.ColumnLabels[column], new ChartPoint(gridLeft + column * cell, gridTop - 5), theme.SecondaryTextColor, 9);
        }

        var legendY = gridTop + _model.Rows * cell + 20;
        canvas.DrawText("Less", new ChartPoint(gridLeft, legendY + 8), theme.SecondaryTextColor, 9);
        for (var i = 0; i < 5; i++)
        {
            var color = i == 0 ? ChartColor.FromHex("#EBEDF0") : LerpColor(_model.MinColor, _model.MaxColor, i / 4f);
            canvas.DrawRoundRect(new ChartRect(gridLeft + 27 + i * 14, legendY, 11, 11), 2, new ChartBrush(color));
        }
        canvas.DrawText("More", new ChartPoint(gridLeft + 101, legendY + 8), theme.SecondaryTextColor, 9);
    }

    private static ChartColor LerpColor(ChartColor a, ChartColor b, float t)
    {
        return ChartColor.FromArgb(
            (byte)Math.Round(a.A + (b.A - a.A) * t),
            (byte)Math.Round(a.R + (b.R - a.R) * t),
            (byte)Math.Round(a.G + (b.G - a.G) * t),
            (byte)Math.Round(a.B + (b.B - a.B) * t));
    }
}
