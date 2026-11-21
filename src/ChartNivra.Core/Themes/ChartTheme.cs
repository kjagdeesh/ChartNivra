namespace ChartNivra.Core.Themes;

using ChartNivra.Core.Primitives;

/// <summary>
/// Color palette model supporting categorical, sequential, and diverging palettes.
/// </summary>
public record ChartPalette(string Name, IReadOnlyList<ChartColor> Colors)
{
    public ChartColor GetColor(int index)
    {
        if (Colors.Count == 0) return ChartColor.Black;
        return Colors[index % Colors.Count];
    }

    public static readonly ChartPalette Vibrant = new("Vibrant", new[]
    {
        ChartColor.FromHex("#3B82F6"), // Blue
        ChartColor.FromHex("#10B981"), // Emerald
        ChartColor.FromHex("#F59E0B"), // Amber
        ChartColor.FromHex("#EF4444"), // Red
        ChartColor.FromHex("#8B5CF6"), // Purple
        ChartColor.FromHex("#EC4899"), // Pink
        ChartColor.FromHex("#06B6D4")  // Cyan
    });

    public static readonly ChartPalette Ocean = new("Ocean", new[]
    {
        ChartColor.FromHex("#0284C7"),
        ChartColor.FromHex("#0EA5E9"),
        ChartColor.FromHex("#38BDF8"),
        ChartColor.FromHex("#7DD3FC"),
        ChartColor.FromHex("#BAE6FD")
    });
}

/// <summary>
/// Theme definition for light, dark, and high-contrast modes.
/// </summary>
public record ChartTheme(
    string Name,
    ChartColor BackgroundColor,
    ChartColor PlotAreaColor,
    ChartColor TextColor,
    ChartColor SecondaryTextColor,
    ChartColor GridLineColor,
    ChartColor AxisLineColor,
    ChartPalette DefaultPalette)
{
    public static readonly ChartTheme Dark = new(
        "Dark",
        BackgroundColor: ChartColor.FromHex("#0F172A"),
        PlotAreaColor: ChartColor.FromHex("#1E293B"),
        TextColor: ChartColor.FromHex("#F8FAFC"),
        SecondaryTextColor: ChartColor.FromHex("#94A3B8"),
        GridLineColor: ChartColor.FromHex("#334155"),
        AxisLineColor: ChartColor.FromHex("#475569"),
        DefaultPalette: ChartPalette.Vibrant
    );

    public static readonly ChartTheme Light = new(
        "Light",
        BackgroundColor: ChartColor.FromHex("#FFFFFF"),
        PlotAreaColor: ChartColor.FromHex("#F8FAFC"),
        TextColor: ChartColor.FromHex("#0F172A"),
        SecondaryTextColor: ChartColor.FromHex("#64748B"),
        GridLineColor: ChartColor.FromHex("#E2E8F0"),
        AxisLineColor: ChartColor.FromHex("#CBD5E1"),
        DefaultPalette: ChartPalette.Vibrant
    );
}
