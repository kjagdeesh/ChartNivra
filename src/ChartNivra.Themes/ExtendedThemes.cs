namespace ChartNivra.Themes;

using ChartNivra.Core.Primitives;
using ChartNivra.Core.Themes;

/// <summary>
/// Predefined palettes for multi-series charts matching high-end design systems.
/// </summary>
public static class ExtendedPalettes
{
    public static readonly ChartPalette Coral = new("Coral", new[]
    {
        ChartColor.FromHex("#F43F5E"),
        ChartColor.FromHex("#FB923C"),
        ChartColor.FromHex("#FBBF24"),
        ChartColor.FromHex("#A855F7"),
        ChartColor.FromHex("#EC4899")
    });

    public static readonly ChartPalette Forest = new("Forest", new[]
    {
        ChartColor.FromHex("#059669"),
        ChartColor.FromHex("#10B981"),
        ChartColor.FromHex("#34D399"),
        ChartColor.FromHex("#0D9488"),
        ChartColor.FromHex("#14B8A6")
    });

    public static readonly ChartPalette Midnight = new("Midnight", new[]
    {
        ChartColor.FromHex("#6366F1"),
        ChartColor.FromHex("#8B5CF6"),
        ChartColor.FromHex("#EC4899"),
        ChartColor.FromHex("#3B82F6"),
        ChartColor.FromHex("#10B981")
    });

    public static readonly ChartPalette Sunset = new("Sunset", new[]
    {
        ChartColor.FromHex("#F59E0B"),
        ChartColor.FromHex("#EA580C"),
        ChartColor.FromHex("#DC2626"),
        ChartColor.FromHex("#9333EA"),
        ChartColor.FromHex("#4F46E5")
    });

    public static readonly ChartPalette Arctic = new("Arctic", new[]
    {
        ChartColor.FromHex("#0284C7"),
        ChartColor.FromHex("#0EA5E9"),
        ChartColor.FromHex("#38BDF8"),
        ChartColor.FromHex("#06B6D4"),
        ChartColor.FromHex("#2DD4BF")
    });

    public static IReadOnlyList<ChartPalette> AllPalettes => new[]
    {
        ChartPalette.Clean,
        ChartPalette.Vibrant,
        ChartPalette.Ocean,
        Coral,
        Forest,
        Midnight,
        Sunset,
        Arctic
    };
}

/// <summary>
/// High-contrast theme compliant with WCAG 2.1 AAA accessibility requirements.
/// </summary>
public static class AccessibleThemes
{
    public static readonly ChartTheme HighContrast = new(
        "HighContrast",
        BackgroundColor: ChartColor.FromHex("#000000"),
        PlotAreaColor: ChartColor.FromHex("#000000"),
        TextColor: ChartColor.FromHex("#FFFFFF"),
        SecondaryTextColor: ChartColor.FromHex("#E2E8F0"),
        GridLineColor: ChartColor.FromHex("#475569"),
        AxisLineColor: ChartColor.FromHex("#FFFFFF"),
        DefaultPalette: new ChartPalette("HighContrast", new[]
        {
            ChartColor.FromHex("#FFFF00"), // Yellow
            ChartColor.FromHex("#00FFFF"), // Cyan
            ChartColor.FromHex("#FF00FF"), // Magenta
            ChartColor.FromHex("#FFFFFF"), // White
            ChartColor.FromHex("#00FF00")  // Lime
        })
    );
}

/// <summary>
/// Rule-based conditional color evaluator for dynamically styling data points.
/// </summary>
public class ConditionalColorRule
{
    public Func<double, bool> Condition { get; }
    public ChartColor TargetColor { get; }

    public ConditionalColorRule(Func<double, bool> condition, ChartColor targetColor)
    {
        Condition = condition;
        TargetColor = targetColor;
    }

    public static ChartColor Evaluate(double value, IEnumerable<ConditionalColorRule> rules, ChartColor fallbackColor)
    {
        foreach (var rule in rules)
        {
            if (rule.Condition(value)) return rule.TargetColor;
        }
        return fallbackColor;
    }
}
