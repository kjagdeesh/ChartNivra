namespace ChartNivra.Gallery.Pages;

using ChartNivra.Core.Charts;
using ChartNivra.Core.Primitives;
using ChartNivra.Core.Series;
using ChartNivra.Core.Themes;
using Microsoft.Maui.Controls;

/// <summary>
/// Themes demo — shows the same chart rendered in Light, Dark, and custom palette modes.
/// </summary>
public class ThemesPage : ContentPage
{
    private static readonly Color PageBg     = Color.FromArgb("#F1F5F9");
    private static readonly Color CardBg     = Colors.White;
    private static readonly Color CardBorder = Color.FromArgb("#E2E8F0");
    private static readonly Color TextDark   = Color.FromArgb("#0F172A");
    private static readonly Color TextMuted  = Color.FromArgb("#64748B");

    public ThemesPage()
    {
        BackgroundColor = PageBg;
        Shell.SetNavBarIsVisible(this, false);
        Title = "Themes";

        var header = new VerticalStackLayout
        {
            Spacing = 4, Padding = new Thickness(20, 20, 20, 0),
            BackgroundColor = CardBg,
            Children =
            {
                new Label { Text = "Themes & Palettes", FontSize = 22, FontAttributes = FontAttributes.Bold, TextColor = TextDark },
                new Label { Text = "Light, Dark, and custom color themes", FontSize = 13, TextColor = TextMuted },
                new BoxView { HeightRequest = 1, BackgroundColor = CardBorder, Margin = new Thickness(0, 12, 0, 0) },
            }
        };

        var themes = new[]
        {
            ("🌙  Dark Theme",   ChartTheme.Dark,       "#00C096"),
            ("☀️  Light Theme",  ChartTheme.Light,      "#6366F1"),
            ("🌊  Ocean Palette", ChartTheme.Dark,       "#0EA5E9"),
        };

        var stacks = new VerticalStackLayout { Spacing = 0 };
        stacks.Add(header);

        foreach (var (label, theme, colorHex) in themes)
        {
            var card = BuildThemeCard(label, theme, colorHex);
            stacks.Add(card);
        }

        // Palette swatches
        stacks.Add(BuildPaletteSection());

        Content = new ScrollView { Content = stacks };
    }

    private static View BuildThemeCard(string label, ChartTheme theme, string colorHex)
    {
        var chart = new ChartNivra.Maui.Controls.ChartView
        {
            HeightRequest   = 200,
            BackgroundColor = Colors.Transparent,
            Theme           = theme,
        };

        var color = ChartColor.FromHex(colorHex);
        chart.Model.Series.Add(new LineSeriesModel
        {
            Title       = "Series",
            Color       = color,
            StrokeWidth = 2f,
            Points      = new System.Collections.Generic.List<ChartPoint>
            {
                new(0,30), new(1,55), new(2,40), new(3,70),
                new(4,60), new(5,85), new(6,75), new(7,100),
            }
        });

        return new Frame
        {
            Margin          = new Thickness(16, 12, 16, 0),
            Padding         = new Thickness(0),
            CornerRadius    = 14,
            HasShadow       = false,
            BorderColor     = Color.FromArgb("#E2E8F0"),
            BackgroundColor = Colors.White,
            Content = new VerticalStackLayout
            {
                Children =
                {
                    new Label
                    {
                        Text      = label,
                        FontSize  = 13,
                        FontAttributes = FontAttributes.Bold,
                        TextColor = Color.FromArgb("#0F172A"),
                        Padding   = new Thickness(16, 12, 16, 8),
                    },
                    new BoxView { HeightRequest = 1, BackgroundColor = Color.FromArgb("#E2E8F0") },
                    chart,
                }
            }
        };
    }

    private static View BuildPaletteSection()
    {
        var palettes = new[]
        {
            ("Clean",   new[] { "#00C096", "#3B82F6", "#8B5CF6", "#F59E0B", "#F43F5E", "#06B6D4", "#6366F1" }),
            ("Vibrant", new[] { "#00C096", "#3B82F6", "#F59E0B", "#EF4444", "#8B5CF6", "#EC4899", "#06B6D4" }),
            ("Ocean",   new[] { "#00C096", "#0EA5E9", "#38BDF8", "#6366F1", "#A78BFA" }),
        };

        var section = new Frame
        {
            Margin          = new Thickness(16, 12, 16, 20),
            Padding         = new Thickness(16),
            CornerRadius    = 14,
            HasShadow       = false,
            BorderColor     = Color.FromArgb("#E2E8F0"),
            BackgroundColor = Colors.White,
        };

        var stack = new VerticalStackLayout { Spacing = 14 };
        stack.Add(new Label
        {
            Text = "Color Palettes",
            FontSize = 14, FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#0F172A"),
        });

        foreach (var (name, colors) in palettes)
        {
            var row = new HorizontalStackLayout { Spacing = 6 };
            var nameLabel = new Label { Text = name, FontSize = 12, TextColor = Color.FromArgb("#64748B"), WidthRequest = 60, VerticalOptions = LayoutOptions.Center };
            row.Add(nameLabel);
            foreach (var hex in colors)
            {
                row.Add(new Frame
                {
                    WidthRequest    = 28,
                    HeightRequest   = 28,
                    CornerRadius    = 6,
                    HasShadow       = false,
                    BackgroundColor = Color.FromArgb(hex),
                    BorderColor     = Colors.Transparent,
                    Padding         = new Thickness(0),
                });
            }
            stack.Add(row);
        }

        section.Content = stack;
        return section;
    }
}
