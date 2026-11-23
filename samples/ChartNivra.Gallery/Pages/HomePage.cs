namespace ChartNivra.Gallery.Pages;

using Microsoft.Maui.Controls;

/// <summary>Welcome / landing page.</summary>
public class HomePage : ContentPage
{
    private static readonly Color PageBg       = Color.FromArgb("#F1F5F9");
    private static readonly Color CardBg       = Colors.White;
    private static readonly Color CardBorder   = Color.FromArgb("#E2E8F0");
    private static readonly Color AccentIndigo = Color.FromArgb("#6366F1");
    private static readonly Color AccentTeal   = Color.FromArgb("#00C096");
    private static readonly Color TextDark     = Color.FromArgb("#0F172A");
    private static readonly Color TextMuted    = Color.FromArgb("#64748B");

    public HomePage()
    {
        BackgroundColor = PageBg;
        Shell.SetNavBarIsVisible(this, false);
        Title = "Home";

        var hero = new Frame
        {
            Margin          = new Thickness(16, 20, 16, 12),
            Padding         = new Thickness(24, 28),
            CornerRadius    = 18,
            HasShadow       = false,
            BorderColor     = Color.FromArgb("#818CF8"),
            BackgroundColor = Color.FromArgb("#EEF2FF"),
            Content = new VerticalStackLayout
            {
                Spacing = 10,
                Children =
                {
                    new Label
                    {
                        Text           = "📊  ChartNivra Gallery",
                        FontSize       = 26,
                        FontAttributes = FontAttributes.Bold,
                        TextColor      = AccentIndigo,
                    },
                    new Label
                    {
                        Text     = "A comprehensive showcase of every chart type available in ChartNivra — built with .NET MAUI and SkiaSharp.",
                        FontSize = 14,
                        TextColor = Color.FromArgb("#4338CA"),
                        LineBreakMode = LineBreakMode.WordWrap,
                    },
                }
            }
        };

        var categoriesLabel = new Label
        {
            Text           = "Categories",
            FontSize       = 16,
            FontAttributes = FontAttributes.Bold,
            TextColor      = TextDark,
            Margin         = new Thickness(20, 8, 20, 10),
        };

        var grid = new Grid
        {
            Margin           = new Thickness(16, 0, 16, 16),
            ColumnSpacing    = 10,
            RowSpacing       = 10,
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star },
            },
        };

        var categories = new[]
        {
            ("📈", "Cartesian",   "Line, Area, Bar, Scatter",           "#00C096"),
            ("🥧", "Circular",    "Pie & Donut charts",                 "#6366F1"),
            ("💹", "Financial",   "Candlestick & OHLC",                 "#F59E0B"),
            ("🎯", "Gauges",      "Radial & arc gauges",                "#EF4444"),
            ("📊", "Statistical", "BoxPlot, Radar, Heatmap",            "#8B5CF6"),
            ("⚡", "Advanced",    "Real-time & Annotations",            "#06B6D4"),
        };

        for (int i = 0; i < categories.Length; i++)
        {
            var (emoji, name, desc, colorHex) = categories[i];
            var card = MakeCategoryCard(emoji, name, desc, Color.FromArgb(colorHex));
            grid.Add(card, i % 2, i / 2);
        }

        var techSection = new Frame
        {
            Margin          = new Thickness(16, 0, 16, 20),
            Padding         = new Thickness(20),
            CornerRadius    = 14,
            HasShadow       = false,
            BorderColor     = CardBorder,
            BackgroundColor = CardBg,
            Content = new VerticalStackLayout
            {
                Spacing = 8,
                Children =
                {
                    new Label
                    {
                        Text           = "Built With",
                        FontSize       = 14,
                        FontAttributes = FontAttributes.Bold,
                        TextColor      = TextDark,
                    },
                    MakeTechRow("🔵", ".NET MAUI 10",    "Cross-platform UI framework"),
                    MakeTechRow("🎨", "SkiaSharp 2.88",  "High-performance 2D rendering"),
                    MakeTechRow("📐", "ChartNivra Core", "Pure C# chart model layer"),
                }
            }
        };

        Content = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                Spacing  = 0,
                Children = { hero, categoriesLabel, grid, techSection }
            }
        };
    }

    private static Frame MakeCategoryCard(string emoji, string name, string desc, Color accent)
    {
        return new Frame
        {
            Padding         = new Thickness(16),
            CornerRadius    = 12,
            HasShadow       = false,
            BorderColor     = Color.FromArgb("#E2E8F0"),
            BackgroundColor = Colors.White,
            Content = new VerticalStackLayout
            {
                Spacing = 6,
                Children =
                {
                    new Label { Text = emoji, FontSize = 24 },
                    new Label
                    {
                        Text           = name,
                        FontSize       = 14,
                        FontAttributes = FontAttributes.Bold,
                        TextColor      = Color.FromArgb("#0F172A"),
                    },
                    new Label
                    {
                        Text      = desc,
                        FontSize  = 11,
                        TextColor = Color.FromArgb("#64748B"),
                        LineBreakMode = LineBreakMode.WordWrap,
                    },
                }
            }
        };
    }

    private static View MakeTechRow(string icon, string name, string detail)
    {
        return new HorizontalStackLayout
        {
            Spacing = 10,
            Children =
            {
                new Label { Text = icon, FontSize = 16, VerticalOptions = LayoutOptions.Center },
                new VerticalStackLayout
                {
                    Spacing = 0,
                    Children =
                    {
                        new Label { Text = name,   FontSize = 13, FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#0F172A") },
                        new Label { Text = detail, FontSize = 11, TextColor = Color.FromArgb("#64748B") },
                    }
                }
            }
        };
    }
}
