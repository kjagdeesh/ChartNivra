namespace ChartNivra.Gallery.Pages;

using ChartNivra.Gallery.Controls;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;

/// <summary>Welcome / landing page - Clean white design with professional MONOCHROME icons.</summary>
public class HomePage : ContentPage
{
    private static readonly Color Background = Colors.White;
    private static readonly Color CardBg = Colors.White;
    private static readonly Color CardBorder = Color.FromArgb("#E2E8F0");
    private static readonly Color Accent = Color.FromArgb("#6366F1");
    private static readonly Color AccentSubtle = Color.FromArgb("#EEF2FF");
    private static readonly Color TextPrimary = Color.FromArgb("#0F172A");
    private static readonly Color TextSecondary = Color.FromArgb("#475569");
    private static readonly Color TextMuted = Color.FromArgb("#94A3B8");
    private readonly Border _hero;
    private readonly Grid _statisticsGrid;
    private readonly Grid _categoriesGrid;
    private readonly Border[] _categoryCards;

    public HomePage()
    {
        BackgroundColor = Background;
        Shell.SetNavBarIsVisible(this, false);
        Title = "Home";

        _hero = BuildHero(out _statisticsGrid);
        var categoriesHeader = BuildCategoriesHeader();
        (_categoriesGrid, _categoryCards) = BuildCategoriesGrid();
        var techSection = BuildTechSection();

        Content = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                Spacing = 0,
                Children = { _hero, categoriesHeader, _categoriesGrid, techSection }
            }
        };

        SizeChanged += (_, _) => UpdateLayoutForWidth(Width);
    }

    private Border BuildHero(out Grid statisticsGrid)
    {
        statisticsGrid = new Grid
        {
            Margin = new Thickness(0, 8, 0, 0),
            ColumnSpacing = 12,
            RowSpacing = 12,
        };

        var hero = new Border
        {
            Margin = new Thickness(24, 32, 24, 24),
            Padding = new Thickness(32, 32),
            StrokeShape = new RoundRectangle { CornerRadius = 16 },
            BackgroundColor = AccentSubtle,
            Stroke = Color.FromArgb("#C7D2FE"),
            StrokeThickness = 1,
            Content = new VerticalStackLayout
            {
                Spacing = 16,
                Children =
                {
                    new HorizontalStackLayout
                    {
                        Spacing = 12,
                        VerticalOptions = LayoutOptions.Center,
                        Children =
                        {
                            new Label
                            {
                                FontFamily = "Segoe UI Symbol, Segoe UI Emoji, Noto Sans Symbols",
                                Text = "\u25B6",
                                FontSize = 32,
                                TextColor = Accent,
                                VerticalOptions = LayoutOptions.Center,
                            },
                            new VerticalStackLayout
                            {
                                Spacing = 2,
                                Children =
                                {
                                    new Label
                                    {
                                        Text = "ChartNivra Gallery",
                                        FontSize = 28,
                                        FontAttributes = FontAttributes.Bold,
                                        TextColor = TextPrimary,
                                    },
                                    new Label
                                    {
                                        Text = "Professional charting library for .NET MAUI",
                                        FontSize = 14,
                                        TextColor = TextSecondary,
                                    },
                                }
                            }
                        }
                    },

                    new Label
                    {
                        Text = "A comprehensive collection of chart types built with .NET MAUI and SkiaSharp \u2014 featuring Cartesian, Circular, Financial, Statistical, and Advanced visualizations.",
                        FontSize = 15,
                        TextColor = TextSecondary,
                        LineBreakMode = LineBreakMode.WordWrap,
                    },

                    statisticsGrid
                }
            }
        };

        AddStatistics(statisticsGrid, 2);
        return hero;
    }

    private Label BuildCategoriesHeader()
    {
        return new Label
        {
            Text = "Chart Categories",
            FontSize = 20,
            FontAttributes = FontAttributes.Bold,
            TextColor = TextPrimary,
            Margin = new Thickness(24, 8, 24, 16),
        };
    }

    private (Grid Grid, Border[] Cards) BuildCategoriesGrid()
    {
        var grid = new Grid
        {
            Margin = new Thickness(24, 0, 24, 24),
            ColumnSpacing = 16,
            RowSpacing = 16,
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star },
            },
        };

        var categories = new[]
        {
            ("line_chart", "Cartesian", "Line, Area, Bar, Scatter"),
            ("pie_chart", "Circular", "Pie & Donut charts"),
            ("candlestick", "Financial", "Candlestick & OHLC"),
            ("radial_gauge", "Gauges", "Radial & arc gauges"),
            ("radar_chart", "Statistical", "Box Plot, Radar, Heatmap"),
            ("realtime", "Advanced", "Real-time & Annotations"),
        };

        var cards = new Border[categories.Length];
        for (int i = 0; i < categories.Length; i++)
        {
            var (icon, name, desc) = categories[i];
            var card = MakeCategoryCard(icon, name, desc);
            cards[i] = card;
        }

        AddCategoryCards(grid, cards, 2);
        return (grid, cards);
    }

    private void UpdateLayoutForWidth(double width)
    {
        if (width <= 0) return;

        var compact = ResponsiveLayout.IsCompact(width);
        _hero.Margin = compact ? new Thickness(12, 16, 12, 16) : new Thickness(24, 32, 24, 24);
        _hero.Padding = compact ? new Thickness(20) : new Thickness(32);
        AddStatistics(_statisticsGrid, compact ? 2 : 4);
        AddCategoryCards(_categoriesGrid, _categoryCards, compact ? 1 : 2);
    }

    private static void AddStatistics(Grid grid, int columns)
    {
        grid.Children.Clear();
        grid.ColumnDefinitions.Clear();
        for (var i = 0; i < columns; i++)
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

        var statistics = new[]
        {
            MakeStatItem("15+", "Chart Types"), MakeStatItem("3", "Categories"),
            MakeStatItem("MAUI", "Native"), MakeStatItem("SkiaSharp", "Rendering"),
        };
        for (var i = 0; i < statistics.Length; i++)
            grid.Add(statistics[i], i % columns, i / columns);
    }

    private static void AddCategoryCards(Grid grid, Border[] cards, int columns)
    {
        grid.Children.Clear();
        grid.ColumnDefinitions.Clear();
        for (var i = 0; i < columns; i++)
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
        for (var i = 0; i < cards.Length; i++)
            grid.Add(cards[i], i % columns, i / columns);
    }

    private Border BuildTechSection()
    {
        return new Border
        {
            Margin = new Thickness(24, 0, 24, 32),
            Padding = new Thickness(24, 20),
            StrokeShape = new RoundRectangle { CornerRadius = 12 },
            BackgroundColor = CardBg,
            Stroke = CardBorder,
            StrokeThickness = 1,
            Content = new VerticalStackLayout
            {
                Spacing = 16,
                Children =
                {
                    new Label
                    {
                        Text = "Technology Stack",
                        FontSize = 16,
                        FontAttributes = FontAttributes.Bold,
                        TextColor = TextPrimary,
                    },
                    MakeTechRow("\u26A1", ".NET MAUI 10", "Cross-platform UI framework"),
                    MakeTechRow("\u25CE", "SkiaSharp 2.88", "High-performance 2D rendering"),
                    MakeTechRow("\u25A1", "ChartNivra Core", "Pure C# chart model layer"),
                }
            }
        };
    }

    private static Border MakeCategoryCard(string iconName, string name, string desc)
    {
        return new Border
        {
            Padding = new Thickness(20),
            StrokeShape = new RoundRectangle { CornerRadius = 12 },
            BackgroundColor = CardBg,
            Stroke = CardBorder,
            StrokeThickness = 1,
            Content = new VerticalStackLayout
            {
                Spacing = 12,
                Children =
                {
                    new Label
                    {
                        FontFamily = "Segoe UI Symbol, Segoe UI Emoji, Noto Sans Symbols",
                        Text = GetIcon(iconName),
                        FontSize = 28,
                        TextColor = Accent,
                        HorizontalOptions = LayoutOptions.Start,
                    },

                    new VerticalStackLayout
                    {
                        Spacing = 4,
                        Children =
                        {
                            new Label
                            {
                                Text = name,
                                FontSize = 16,
                                FontAttributes = FontAttributes.Bold,
                                TextColor = TextPrimary,
                            },
                            new Label
                            {
                                Text = desc,
                                FontSize = 13,
                                TextColor = TextSecondary,
                                LineBreakMode = LineBreakMode.WordWrap,
                            }
                        }
                    }
                }
            }
        };
    }

    private static Border MakeStatItem(string value, string label)
    {
        return new Border
        {
            Padding = new Thickness(16, 12),
            StrokeShape = new RoundRectangle { CornerRadius = 8 },
            BackgroundColor = Colors.White,
            Stroke = CardBorder,
            StrokeThickness = 1,
            Content = new VerticalStackLayout
            {
                Spacing = 2,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    new Label
                    {
                        Text = value,
                        FontSize = 18,
                        FontAttributes = FontAttributes.Bold,
                        TextColor = TextPrimary,
                        HorizontalOptions = LayoutOptions.Center,
                    },
                    new Label
                    {
                        Text = label,
                        FontSize = 11,
                        TextColor = TextMuted,
                        HorizontalOptions = LayoutOptions.Center,
                    }
                }
            }
        };
    }

    private static View MakeTechRow(string icon, string name, string detail)
    {
        return new HorizontalStackLayout
        {
            Spacing = 12,
            Children =
            {
                new Label
                {
                    FontFamily = "Segoe UI Symbol, Segoe UI Emoji, Noto Sans Symbols",
                    Text = icon,
                    FontSize = 20,
                    TextColor = Accent,
                    VerticalOptions = LayoutOptions.Center,
                    WidthRequest = 28,
                },
                new VerticalStackLayout
                {
                    Spacing = 2,
                    Children =
                    {
                        new Label { Text = name, FontSize = 13, FontAttributes = FontAttributes.Bold, TextColor = TextPrimary },
                        new Label { Text = detail, FontSize = 12, TextColor = TextMuted },
                    }
                }
            }
        };
    }

    private static string GetIcon(string iconName)
    {
        // MONOCHROME only - no color emojis
        return iconName switch
        {
            "line_chart" => "\u25B6",
            "pie_chart" => "\u229A",
            "candlestick" => "\u25B2\u25BC",
            "radial_gauge" => "\u23F0",
            "radar_chart" => "\u2315",
            "realtime" => "\u26A1",
            _ => "\u25B6",
        };
    }
}
