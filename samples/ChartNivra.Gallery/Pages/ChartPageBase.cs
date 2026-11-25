namespace ChartNivra.Gallery.Pages;

using ChartNivra.Gallery.Controls;
using ChartNivra.Maui.Controls;
using ChartNivra.Rendering.Abstractions;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;

/// <summary>Responsive chart-and-code workspace shared by all interactive demos.</summary>
public abstract class ChartPageBase : ContentPage
{
    protected static readonly Color PageBg = Color.FromArgb("#F8FAFC");
    protected static readonly Color CardBg = Colors.White;
    protected static readonly Color CardBorder = Color.FromArgb("#E2E8F0");
    protected static readonly Color AccentIndigo = Color.FromArgb("#4F46E5");
    protected static readonly Color TextDark = Color.FromArgb("#0F172A");
    protected static readonly Color TextMuted = Color.FromArgb("#64748B");

    protected readonly ChartView ChartView;
    private readonly CodeSnippetView _csharpSnippet;
    private readonly CodeSnippetView _xamlSnippet;
    private readonly Button _csharpTab;
    private readonly Button _xamlTab;
    private readonly ContentView _codeContent;
    private readonly Border _chartPanel;
    private readonly Border _codePanel;
    private readonly Grid _workspace;
    private readonly VerticalStackLayout _pageContent;
    private readonly VerticalStackLayout _previewContent;
    private bool? _compactLayout;

    protected ChartPageBase(string title, string subtitle, string csharpCode)
    {
        Title = title;
        BackgroundColor = PageBg;
        Shell.SetNavBarIsVisible(this, false);

        ChartView = new ChartView { HeightRequest = 430, BackgroundColor = Colors.Transparent };
        _csharpSnippet = new CodeSnippetView { Code = csharpCode, Language = CodeLanguage.CSharp };
        _xamlSnippet = new CodeSnippetView { Code = SampleXaml.Host, Language = CodeLanguage.Xaml };
        _csharpTab = MakeTab("C#", true);
        _xamlTab = MakeTab("XAML", false);
        _csharpTab.Clicked += (_, _) => SelectCode(false);
        _xamlTab.Clicked += (_, _) => SelectCode(true);
        _codeContent = new ContentView { Content = _csharpSnippet };

        var previewHeading = new Grid { ColumnDefinitions = new ColumnDefinitionCollection { new() { Width = GridLength.Star }, new() { Width = GridLength.Auto } } };
        previewHeading.Add(new VerticalStackLayout { Spacing = 4, Children = { new Label { Text = "LIVE EXAMPLE", FontSize = 10, FontAttributes = FontAttributes.Bold, CharacterSpacing = 1.2, TextColor = AccentIndigo }, new Label { Text = "See it in action", FontSize = 20, FontAttributes = FontAttributes.Bold, TextColor = TextDark } } }, 0, 0);
        previewHeading.Add(new Border { Padding = new Thickness(10, 0), HeightRequest = 28, VerticalOptions = LayoutOptions.Center, BackgroundColor = Color.FromArgb("#ECFDF5"), StrokeThickness = 0, StrokeShape = new RoundRectangle { CornerRadius = 14 }, Content = new Label { Text = "●  LIVE", FontSize = 10, FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#059669"), VerticalTextAlignment = TextAlignment.Center } }, 1, 0);
        var stage = new Border { Padding = new Thickness(14), BackgroundColor = Colors.White, Stroke = Color.FromArgb("#E7ECF4"), StrokeThickness = 1, StrokeShape = new RoundRectangle { CornerRadius = 14 }, Content = ChartView };
        _previewContent = new VerticalStackLayout { Spacing = 20, Children = { previewHeading, stage, new Label { Text = "A closer look at the sample data.", FontSize = 12, TextColor = TextMuted } } };
        _chartPanel = new Border
        {
            Padding = new Thickness(24), BackgroundColor = Color.FromArgb("#F0F3FA"), StrokeThickness = 0,
            StrokeShape = new RoundRectangle { CornerRadius = 20 },
            Content = _previewContent,
        };
        var tabBar = new Grid { ColumnDefinitions = new ColumnDefinitionCollection { new() { Width = new GridLength(62) }, new() { Width = new GridLength(62) } }, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Start, ColumnSpacing = 2, Padding = 3, BackgroundColor = Color.FromArgb("#EFF4FF"), HeightRequest = 40 };
        tabBar.Add(_csharpTab, 0, 0); tabBar.Add(_xamlTab, 1, 0);
        var inspectorHeader = new Grid { Padding = new Thickness(24, 22, 24, 16), ColumnSpacing = 12, ColumnDefinitions = new ColumnDefinitionCollection { new() { Width = GridLength.Star }, new() { Width = GridLength.Auto } } };
        inspectorHeader.Add(new VerticalStackLayout { Spacing = 4, Children = { new Label { Text = "MAKE IT YOURS", FontSize = 10, FontAttributes = FontAttributes.Bold, CharacterSpacing = 1, TextColor = AccentIndigo }, new Label { Text = "Explore the code", FontSize = 20, FontAttributes = FontAttributes.Bold, TextColor = TextDark }, new Label { Text = "Switch languages and copy the sample.", FontSize = 12, TextColor = TextMuted } } }, 0, 0);
        inspectorHeader.Add(new Border { StrokeThickness = 0, BackgroundColor = Color.FromArgb("#EFF4FF"), StrokeShape = new RoundRectangle { CornerRadius = 10 }, Content = tabBar, VerticalOptions = LayoutOptions.Start }, 1, 0);
        _codePanel = new Border
        {
            Padding = 0, BackgroundColor = CardBg, Stroke = CardBorder, StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 20 },
            Content = new VerticalStackLayout
            {
                Spacing = 0,
                Children =
                {
                    inspectorHeader,
                    new ContentView { Padding = new Thickness(20, 0, 20, 20), Content = _codeContent },
                }
            }
        };

        _workspace = new Grid { ColumnSpacing = 20, RowSpacing = 20 };
        var backButton = GalleryTheme.BackButton();
        var header = new Grid
        {
            RowSpacing = 18,
            RowDefinitions = new RowDefinitionCollection { new() { Height = GridLength.Auto }, new() { Height = GridLength.Auto } },
            Margin = new Thickness(0, 0, 0, 12),
        };
        header.Add(backButton, 0, 0);
        header.Add(new VerticalStackLayout
        {
            Spacing = 1,
            Children =
            {
                new Label { Text = "CHARTNIVRA / SAMPLE", FontSize = 10, FontAttributes = FontAttributes.Bold, CharacterSpacing = 1, TextColor = AccentIndigo },
                new Label { Text = title, FontSize = 23, FontAttributes = FontAttributes.Bold, TextColor = TextDark },
            }
        }, 0, 1);
        // A single editor-like workspace: navigation lives in a slim top bar, the chart
        // occupies the main stage, and the source panel acts as a companion inspector.
        var titleLabel = new Label { Text = title, FontSize = 30, FontAttributes = FontAttributes.Bold, TextColor = TextDark };
        titleLabel.SetAppThemeColor(Label.TextColorProperty, TextDark, Color.FromArgb("#F1F5F9"));
        var intro = new VerticalStackLayout
        {
            Spacing = 5, Margin = new Thickness(0, 12, 0, 22),
            Children =
            {
                new Label { Text = "SAMPLE LIBRARY  /  CHART", FontSize = 10, FontAttributes = FontAttributes.Bold, CharacterSpacing = 1.3, TextColor = AccentIndigo },
                titleLabel,
                new Label { Text = subtitle, FontSize = 14, TextColor = TextMuted },
            }
        };
        header.Children.Clear();
        header.RowDefinitions.Clear();
        header.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
        header.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        header.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        header.Margin = new Thickness(0, 0, 0, 10);
        header.Add(new BrandView(), 0, 0);
        header.Add(backButton, 1, 0);
        var copyButton = new Button { Text = "Copy C#", FontSize = 13, FontAttributes = FontAttributes.Bold, TextColor = Colors.White, BackgroundColor = Color.FromArgb("#4338CA"), CornerRadius = 8, HeightRequest = 42, Padding = new Thickness(16, 0), Margin = new Thickness(10, 0, 0, 0) };
        copyButton.Clicked += async (_, _) => { await Clipboard.Default.SetTextAsync(_csharpSnippet.Code); copyButton.Text = "Copied"; await Task.Delay(1600); copyButton.Text = "Copy C#"; };
        header.Add(copyButton, 2, 0);
        _pageContent = new VerticalStackLayout
        {
            Spacing = 0, Padding = new Thickness(24, 20, 24, 28),
            Children = { header, new BoxView { HeightRequest = 1, BackgroundColor = CardBorder }, intro, _workspace }
        };
        Content = new ScrollView { Content = _pageContent };
        ConfigureChart();
        ChartView.SetAppTheme(ChartView.ThemeProperty, ChartNivra.Core.Themes.ChartTheme.Light, ChartNivra.Core.Themes.ChartTheme.Dark);
        this.SetAppThemeColor(BackgroundColorProperty, PageBg, Color.FromArgb("#0B0B0C"));
        GalleryTheme.Apply(_pageContent);
        GalleryTheme.Apply(_chartPanel);
        GalleryTheme.Apply(_codePanel);
        SizeChanged += (_, _) => UpdateLayout(Width);
    }

    protected abstract void ConfigureChart();
    protected void UseRenderer(IChartRenderer renderer) => ChartView.Renderer = renderer;
    protected void AddPreviewControl(View view) => _previewContent.Add(view);

    private void UpdateLayout(double width)
    {
        if (width <= 0) return;
        var compact = ResponsiveLayout.IsCompact(width);
        _pageContent.Padding = compact ? new Thickness(12, 16, 12, 20) : new Thickness(24, 20, 24, 28);
        _pageContent.WidthRequest = width > 1460 ? 1360 : -1;
        if (_compactLayout == compact) return;
        _compactLayout = compact;
        _workspace.Children.Clear(); _workspace.ColumnDefinitions.Clear(); _workspace.RowDefinitions.Clear();
        if (compact)
        {
            ChartView.HeightRequest = 250; _codePanel.HeightRequest = 480;
            _csharpSnippet.HeightRequest = _xamlSnippet.HeightRequest = 325;
            _workspace.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            _workspace.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            _workspace.Add(_chartPanel, 0, 0); _workspace.Add(_codePanel, 0, 1);
        }
        else
        {
            ChartView.HeightRequest = 380; _codePanel.HeightRequest = 560;
            _csharpSnippet.HeightRequest = _xamlSnippet.HeightRequest = 405;
            _workspace.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) });
            _workspace.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });
            _workspace.Add(_chartPanel, 0, 0); _workspace.Add(_codePanel, 1, 0);
        }
    }

    private void SelectCode(bool xaml)
    {
        _codeContent.Content = xaml ? _xamlSnippet : _csharpSnippet;
        UpdateTab(_csharpTab, !xaml); UpdateTab(_xamlTab, xaml);
    }

    private static Button MakeTab(string text, bool active)
    {
        var tab = new Button { Text = text, FontSize = 12, FontAttributes = FontAttributes.Bold, Padding = 0, HeightRequest = 34, CornerRadius = 7, BorderWidth = 0 };
        UpdateTab(tab, active); return tab;
    }

    private static void UpdateTab(Button tab, bool active)
    {
        GalleryTheme.SetCodeTab(tab, active);
    }

    private static TapGestureRecognizer TapGesture(Action action)
    {
        var gesture = new TapGestureRecognizer(); gesture.Tapped += (_, _) => action(); return gesture;
    }

}
