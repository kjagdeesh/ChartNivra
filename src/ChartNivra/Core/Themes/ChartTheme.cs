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

    /// <summary>
    /// Modern SaaS Clean Palette (Teal, Cyan, Purple, Amber, Rose, Indigo)
    /// Matches Pulsey and Dribbble reference designs.
    /// </summary>
    public static readonly ChartPalette Clean = new("Clean", new[]
    {
        ChartColor.FromHex("#00C096"), // Clean Teal / Mint
        ChartColor.FromHex("#3B82F6"), // Electric Blue
        ChartColor.FromHex("#8B5CF6"), // Smooth Purple
        ChartColor.FromHex("#F59E0B"), // Warm Amber
        ChartColor.FromHex("#F43F5E"), // Coral Rose
        ChartColor.FromHex("#06B6D4"), // Sky Cyan
        ChartColor.FromHex("#6366F1")  // Soft Indigo
    });

    public static readonly ChartPalette Vibrant = new("Vibrant", new[]
    {
        ChartColor.FromHex("#00C096"),
        ChartColor.FromHex("#3B82F6"),
        ChartColor.FromHex("#F59E0B"),
        ChartColor.FromHex("#EF4444"),
        ChartColor.FromHex("#8B5CF6"),
        ChartColor.FromHex("#EC4899"),
        ChartColor.FromHex("#06B6D4")
    });

    public static readonly ChartPalette Ocean = new("Ocean", new[]
    {
        ChartColor.FromHex("#00C096"),
        ChartColor.FromHex("#0EA5E9"),
        ChartColor.FromHex("#38BDF8"),
        ChartColor.FromHex("#6366F1"),
        ChartColor.FromHex("#A78BFA")
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
    /// <summary>
    /// Light Theme designed after Dribbble and Modern Health Dashboard aesthetics
    /// (Crisp white cards, subtle borders #E2E8F0, dark slate text, mint/cyan/purple accents)
    /// </summary>
    public static readonly ChartTheme CleanLight = new(
        "CleanLight",
        BackgroundColor: ChartColor.FromHex("#FFFFFF"),
        PlotAreaColor: ChartColor.FromHex("#FAFCFD"),
        TextColor: ChartColor.FromHex("#0F172A"),
        SecondaryTextColor: ChartColor.FromHex("#64748B"),
        GridLineColor: ChartColor.FromHex("#F1F5F9"),
        AxisLineColor: ChartColor.FromHex("#E2E8F0"),
        DefaultPalette: ChartPalette.Clean
    );

    public static readonly ChartTheme Light = CleanLight;

    public static readonly ChartTheme Dark = new(
        "Dark",
        BackgroundColor: ChartColor.FromHex("#0F172A"),
        PlotAreaColor: ChartColor.FromHex("#1E293B"),
        TextColor: ChartColor.FromHex("#F8FAFC"),
        SecondaryTextColor: ChartColor.FromHex("#94A3B8"),
        GridLineColor: ChartColor.FromHex("#334155"),
        AxisLineColor: ChartColor.FromHex("#475569"),
        DefaultPalette: ChartPalette.Clean
    );
}
