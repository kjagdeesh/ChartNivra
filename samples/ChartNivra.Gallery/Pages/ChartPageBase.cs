namespace ChartNivra.Gallery.Pages;

using ChartNivra.Gallery.Controls;
using ChartNivra.Maui.Controls;
using ChartNivra.Rendering.Abstractions;
using Microsoft.Maui.Controls;

/// <summary>
/// Base class for all chart demo pages. Provides the standard two-panel layout:
/// top = live chart, bottom = tabbed "Preview / Code" toggle.
/// </summary>
public abstract class ChartPageBase : ContentPage
{
    // ── Design tokens ────────────────────────────────────────────────────────
    protected static readonly Color PageBg       = Color.FromArgb("#F1F5F9");
    protected static readonly Color CardBg       = Colors.White;
    protected static readonly Color CardBorder   = Color.FromArgb("#E2E8F0");
    protected static readonly Color AccentIndigo = Color.FromArgb("#6366F1");
    protected static readonly Color TextDark     = Color.FromArgb("#0F172A");
    protected static readonly Color TextMuted    = Color.FromArgb("#64748B");

    protected readonly ChartView ChartView;
    private   readonly CodeSnippetView _snippet;
    private   readonly Label           _previewTab;
    private   readonly Label           _codeTab;
    private   readonly ContentView     _tabContent;
    private   bool _showingCode = false;

    protected ChartPageBase(string title, string subtitle, string codeSnippet)
    {
        BackgroundColor = PageBg;
        Shell.SetNavBarIsVisible(this, false);

        ChartView = new ChartView
        {
            HeightRequest   = 320,
            BackgroundColor = Colors.Transparent,
        };

        _snippet = new CodeSnippetView
        {
            Code            = codeSnippet,
            HeightRequest   = 260,
        };

        _previewTab = MakeTab("Preview", true);
        _codeTab    = MakeTab("Code",    false);

        _previewTab.GestureRecognizers.Add(TapGesture(() => SwitchTab(false)));
        _codeTab.GestureRecognizers.Add(TapGesture(() => SwitchTab(true)));

        _tabContent = new ContentView { Content = new BoxView() };

        var tabBar = new Grid
        {
            ColumnSpacing   = 0,
            BackgroundColor = CardBg,
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star },
            },
        };
        tabBar.Add(_previewTab, 0, 0);
        tabBar.Add(_codeTab,    1, 0);

        // Page header
        var headerSection = new VerticalStackLayout
        {
            Spacing         = 4,
            Padding         = new Thickness(20, 20, 20, 0),
            BackgroundColor = CardBg,
            Children =
            {
                new Label
                {
                    Text           = title,
                    FontSize       = 22,
                    FontAttributes = FontAttributes.Bold,
                    TextColor      = TextDark,
                },
                new Label
                {
                    Text      = subtitle,
                    FontSize  = 13,
                    TextColor = TextMuted,
                },
                new BoxView
                {
                    HeightRequest   = 1,
                    BackgroundColor = CardBorder,
                    Margin          = new Thickness(0, 12, 0, 0),
                }
            }
        };

        // Chart card
        var chartCard = new Frame
        {
            Margin          = new Thickness(16, 12),
            Padding         = new Thickness(4),
            CornerRadius    = 14,
            HasShadow       = false,
            BorderColor     = CardBorder,
            BackgroundColor = CardBg,
            Content         = ChartView,
        };

        // Code/Preview tabs section
        var tabSection = new Frame
        {
            Margin          = new Thickness(16, 0, 16, 16),
            Padding         = new Thickness(0),
            CornerRadius    = 14,
            HasShadow       = false,
            BorderColor     = CardBorder,
            BackgroundColor = CardBg,
            Content = new VerticalStackLayout
            {
                Spacing  = 0,
                Children =
                {
                    tabBar,
                    new BoxView { HeightRequest = 1, BackgroundColor = CardBorder },
                    new ContentView
                    {
                        Padding = new Thickness(12),
                        Content = _tabContent,
                    }
                }
            }
        };

        Content = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                Spacing  = 0,
                Children =
                {
                    headerSection,
                    chartCard,
                    tabSection,
                }
            }
        };

        // Show preview by default
        SwitchTab(false);
        // Allow derived classes to configure ChartView
        ConfigureChart();
    }

    /// <summary>Derived classes configure the ChartView / Renderer here.</summary>
    protected abstract void ConfigureChart();

    private void SwitchTab(bool showCode)
    {
        _showingCode = showCode;

        // Update tab styles
        UpdateTabStyle(_previewTab, !showCode);
        UpdateTabStyle(_codeTab,    showCode);

        _tabContent.Content = showCode
            ? _snippet
            : BuildPreviewPlaceholder();
    }

    private static View BuildPreviewPlaceholder()
    {
        return new Label
        {
            Text              = "Chart is rendered above ↑",
            FontSize          = 13,
            TextColor         = Color.FromArgb("#94A3B8"),
            HorizontalOptions = LayoutOptions.Center,
            Margin            = new Thickness(0, 24),
        };
    }

    private static Label MakeTab(string text, bool isActive)
    {
        var lbl = new Label
        {
            Text              = text,
            FontSize          = 13,
            FontAttributes    = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center,
            Padding           = new Thickness(0, 12),
        };
        UpdateTabStyle(lbl, isActive);
        return lbl;
    }

    private static void UpdateTabStyle(Label lbl, bool isActive)
    {
        lbl.TextColor = isActive ? Color.FromArgb("#6366F1") : Color.FromArgb("#64748B");
    }

    private static TapGestureRecognizer TapGesture(Action action)
    {
        var tgr = new TapGestureRecognizer();
        tgr.Tapped += (_, _) => action();
        return tgr;
    }

    // Helper to quickly create an IChartRenderer wrapper
    protected void UseRenderer(IChartRenderer renderer) => ChartView.Renderer = renderer;
}
