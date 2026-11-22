namespace ChartNivra.VisualTests;

using ChartNivra.Core.Primitives;
using ChartNivra.Core.Themes;
using ChartNivra.Themes;
using Xunit;

public class VisualTests
{
    [Fact]
    public void CleanLightTheme_HasAccessibleContrast()
    {
        var theme = ChartTheme.CleanLight;
        Assert.Equal("CleanLight", theme.Name);
        Assert.Equal(ChartColor.FromHex("#FFFFFF"), theme.BackgroundColor);
        Assert.Equal(ChartColor.FromHex("#0F172A"), theme.TextColor);
    }

    [Fact]
    public void HighContrastTheme_MeetsAaaRequirements()
    {
        var theme = AccessibleThemes.HighContrast;
        Assert.Equal("HighContrast", theme.Name);
        Assert.Equal(ChartColor.FromHex("#000000"), theme.BackgroundColor);
        Assert.Equal(ChartColor.FromHex("#FFFFFF"), theme.TextColor);
    }

    [Fact]
    public void ConditionalColorRules_EvaluateProperly()
    {
        var rules = new[]
        {
            new ConditionalColorRule(val => val > 80, ChartColor.FromHex("#EF4444")), // Danger Red
            new ConditionalColorRule(val => val > 50, ChartColor.FromHex("#F59E0B"))  // Warning Amber
        };

        var fallback = ChartColor.FromHex("#10B981"); // Normal Green

        var colorHigh = ConditionalColorRule.Evaluate(95, rules, fallback);
        var colorMid = ConditionalColorRule.Evaluate(65, rules, fallback);
        var colorLow = ConditionalColorRule.Evaluate(25, rules, fallback);

        Assert.Equal(ChartColor.FromHex("#EF4444"), colorHigh);
        Assert.Equal(ChartColor.FromHex("#F59E0B"), colorMid);
        Assert.Equal(fallback, colorLow);
    }
}
