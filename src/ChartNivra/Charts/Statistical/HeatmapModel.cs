namespace ChartNivra.Statistical;

using ChartNivra.Core.Primitives;

/// <summary>
/// Mode for heatmap visualization (Matrix grid or Calendar commit style).
/// </summary>
public enum HeatmapMode { Matrix, Calendar }

/// <summary>
/// Single cell in a heatmap grid.
/// </summary>
public record HeatmapCell(int Row, int Column, double Value, string? Label = null);

/// <summary>
/// Heatmap model supporting continuous color interpolation between Low and High colors.
/// </summary>
public class HeatmapModel
{
    public string Title { get; set; } = "Heatmap Analysis";
    public ChartNivra.Core.Themes.ChartTheme Theme { get; set; } = ChartNivra.Core.Themes.ChartTheme.Light;
    public List<string> RowLabels { get; set; } = new();
    public List<string> ColumnLabels { get; set; } = new();
    public HeatmapMode Mode { get; set; } = HeatmapMode.Matrix;
    public int Rows { get; set; }
    public int Columns { get; set; }
    public List<HeatmapCell> Cells { get; set; } = new();
    public ChartColor MinColor { get; set; } = ChartColor.FromHex("#E0F2FE"); // Light sky
    public ChartColor MaxColor { get; set; } = ChartColor.FromHex("#0284C7"); // Deep blue
    public double MinValue { get; set; } = 0.0;
    public double MaxValue { get; set; } = 100.0;

    public ChartColor GetCellColor(double value)
    {
        if (MaxValue <= MinValue) return MinColor;
        var t = Math.Clamp((value - MinValue) / (MaxValue - MinValue), 0.0, 1.0);
        var r = (byte)(MinColor.R + t * (MaxColor.R - MinColor.R));
        var g = (byte)(MinColor.G + t * (MaxColor.G - MinColor.G));
        var b = (byte)(MinColor.B + t * (MaxColor.B - MinColor.B));
        return new ChartColor(r, g, b);
    }
}
