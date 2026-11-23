namespace ChartNivra.Gallery;

using ChartNivra.Gallery.Pages;
using Microsoft.Maui.Controls;

/// <summary>
/// Shell with flyout side-menu navigation grouping all chart examples.
/// </summary>
public class AppShell : Shell
{
    // ── Palette / Design Tokens ─────────────────────────────────────────────
    private static readonly Color BgDark       = Color.FromArgb("#0F172A");
    private static readonly Color BgCard       = Color.FromArgb("#1E293B");
    private static readonly Color AccentIndigo = Color.FromArgb("#6366F1");
    private static readonly Color TextLight    = Color.FromArgb("#F8FAFC");
    private static readonly Color TextMuted    = Color.FromArgb("#94A3B8");
    private static readonly Color BorderColor  = Color.FromArgb("#334155");

    public AppShell()
    {
        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Flyout);

        FlyoutWidth        = 268;
        FlyoutBackgroundColor = BgDark;

        // ── Flyout Header ───────────────────────────────────────────────────
        FlyoutHeader = BuildFlyoutHeader();

        // ── Flyout Footer ───────────────────────────────────────────────────
        FlyoutFooter = BuildFlyoutFooter();

        // ── Navigation groups ───────────────────────────────────────────────
        AddGroup("🏠  Overview", new[] { ("Home", typeof(HomePage)) });

        AddGroup("📈  Cartesian Charts", new[]
        {
            ("Line Chart",       typeof(LineChartPage)),
            ("Multi-Series",     typeof(MultiSeriesPage)),
        });

        AddGroup("🥧  Circular Charts", new[]
        {
            ("Pie Chart",        typeof(PieChartPage)),
            ("Donut Chart",      typeof(DonutChartPage)),
        });

        AddGroup("💹  Financial Charts", new[]
        {
            ("Candlestick",      typeof(CandlestickPage)),
        });

        AddGroup("🎯  Gauges", new[]
        {
            ("Radial Gauge",     typeof(GaugePage)),
        });

        AddGroup("📊  Statistical", new[]
        {
            ("Box Plot",         typeof(BoxPlotPage)),
            ("Radar Chart",      typeof(RadarChartPage)),
            ("Heatmap",          typeof(HeatmapPage)),
        });

        AddGroup("⚡  Advanced", new[]
        {
            ("Real-Time",        typeof(RealTimePage)),
            ("Annotations",      typeof(AnnotationsPage)),
        });

        AddGroup("🎨  Themes & Palettes", new[]
        {
            ("Themes",           typeof(ThemesPage)),
        });

        // Set Home as the initial route
        CurrentItem = Items.OfType<FlyoutItem>().FirstOrDefault() ??
                      Items.FirstOrDefault();
    }

    // ────────────────────────────────────────────────────────────────────────
    //  Helpers
    // ────────────────────────────────────────────────────────────────────────

    private void AddGroup(string groupTitle, (string title, Type pageType)[] entries)
    {
        foreach (var (title, pageType) in entries)
        {
            var flyout = new FlyoutItem
            {
                Title            = title,
                FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
            };

            var tab = new Tab { Title = title };
            var page = new ShellContent
            {
                Title           = title,
                ContentTemplate = new DataTemplate(pageType),
            };
            tab.Items.Add(page);
            flyout.Items.Add(tab);

            // Group label for first item in group
            if (entries[0].title == title)
            {
                Shell.SetFlyoutItemIsVisible(flyout, true);
            }

            Items.Add(flyout);
        }

        // Add a visual separator using a disabled FlyoutItem
        var separator = new FlyoutItem
        {
            Title = groupTitle,
            IsEnabled = false,
        };
        separator.SetValue(Shell.FlyoutItemIsVisibleProperty, false);
        Items.Add(separator);
    }

    private static View BuildFlyoutHeader()
    {
        var container = new Grid
        {
            Padding         = new Thickness(20, 44, 20, 20),
            BackgroundColor = Color.FromArgb("#0A0F1E"),
            RowDefinitions  = new RowDefinitionCollection
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
            },
        };

        // Logo row
        var logoBox = new Frame
        {
            CornerRadius    = 14,
            Padding         = new Thickness(10),
            BackgroundColor = AccentIndigo,
            HasShadow       = false,
            HeightRequest   = 48,
            WidthRequest    = 48,
            HorizontalOptions = LayoutOptions.Start,
            Content = new Label
            {
                Text              = "CN",
                FontSize          = 16,
                FontAttributes    = FontAttributes.Bold,
                TextColor         = Colors.White,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions   = LayoutOptions.Center,
            }
        };
        container.Add(logoBox, 0, 0);

        var title = new Label
        {
            Text           = "ChartNivra",
            FontSize       = 20,
            FontAttributes = FontAttributes.Bold,
            TextColor      = TextLight,
            Margin         = new Thickness(0, 12, 0, 2),
        };
        container.Add(title, 0, 1);

        var subtitle = new Label
        {
            Text      = "Gallery • v1.0",
            FontSize  = 12,
            TextColor = TextMuted,
        };
        container.Add(subtitle, 0, 2);

        // Bottom divider
        var divider = new BoxView
        {
            HeightRequest   = 1,
            BackgroundColor = BorderColor,
            Margin          = new Thickness(0, 16, 0, 0),
        };
        var wrapper = new VerticalStackLayout { Spacing = 0, BackgroundColor = Color.FromArgb("#0A0F1E") };
        wrapper.Add(container);
        wrapper.Add(divider);
        return wrapper;
    }

    private static View BuildFlyoutFooter()
    {
        return new Label
        {
            Text              = "© 2024 ChartNivra",
            FontSize          = 11,
            TextColor         = TextMuted,
            HorizontalOptions = LayoutOptions.Center,
            Padding           = new Thickness(0, 12, 0, 20),
        };
    }
}
