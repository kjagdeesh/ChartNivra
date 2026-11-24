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
    private readonly Label _csharpTab;
    private readonly Label _xamlTab;
    private readonly ContentView _codeContent;
    private readonly Border _chartPanel;
    private readonly Border _codePanel;
    private readonly Grid _workspace;
    private readonly VerticalStackLayout _pageContent;

    protected ChartPageBase(string title, string subtitle, string csharpCode)
    {
        Title = title;
        BackgroundColor = PageBg;
        Shell.SetNavBarIsVisible(this, true);

        ChartView = new ChartView { HeightRequest = 430, BackgroundColor = Colors.Transparent };
        _csharpSnippet = new CodeSnippetView { Code = csharpCode, Language = CodeLanguage.CSharp };
        _xamlSnippet = new CodeSnippetView { Code = CreateXamlSnippet(title), Language = CodeLanguage.Xaml };
        _csharpTab = MakeTab("C#", true);
        _xamlTab = MakeTab("XAML", false);
        _csharpTab.GestureRecognizers.Add(TapGesture(() => SelectCode(false)));
        _xamlTab.GestureRecognizers.Add(TapGesture(() => SelectCode(true)));
        _codeContent = new ContentView { Content = _csharpSnippet };

        _chartPanel = new Border
        {
            Padding = new Thickness(12), BackgroundColor = CardBg, Stroke = CardBorder, StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 14 }, Content = ChartView,
        };
        var tabBar = new Grid { ColumnDefinitions = new ColumnDefinitionCollection { new() { Width = GridLength.Star }, new() { Width = GridLength.Star } } };
        tabBar.Add(_csharpTab, 0, 0); tabBar.Add(_xamlTab, 1, 0);
        _codePanel = new Border
        {
            Padding = 0, BackgroundColor = CardBg, Stroke = CardBorder, StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 14 },
            Content = new VerticalStackLayout
            {
                Spacing = 0,
                Children =
                {
                    new Label { Text = "IMPLEMENTATION · LIVE SAMPLE DATA", FontSize = 10, FontAttributes = FontAttributes.Bold, CharacterSpacing = 1, TextColor = TextMuted, Padding = new Thickness(16, 14, 16, 4) },
                    tabBar,
                    new BoxView { HeightRequest = 1, BackgroundColor = CardBorder },
                    _codeContent,
                }
            }
        };

        _workspace = new Grid { ColumnSpacing = 16, RowSpacing = 16 };
        _pageContent = new VerticalStackLayout
        {
            Spacing = 0, Padding = new Thickness(24, 20, 24, 28),
            Children =
            {
                new Label { Text = title, FontSize = 25, FontAttributes = FontAttributes.Bold, TextColor = TextDark },
                new Label { Text = subtitle, FontSize = 14, TextColor = TextMuted, Margin = new Thickness(0, 4, 0, 20) },
                _workspace,
            }
        };
        Content = new ScrollView { Content = _pageContent };
        ConfigureChart();
        SizeChanged += (_, _) => UpdateLayout(Width);
    }

    protected abstract void ConfigureChart();
    protected void UseRenderer(IChartRenderer renderer) => ChartView.Renderer = renderer;

    private void UpdateLayout(double width)
    {
        if (width <= 0) return;
        var compact = ResponsiveLayout.IsCompact(width);
        _pageContent.Padding = compact ? new Thickness(12, 16, 12, 20) : new Thickness(24, 20, 24, 28);
        _pageContent.WidthRequest = width > 1460 ? 1360 : -1;
        _workspace.Children.Clear(); _workspace.ColumnDefinitions.Clear(); _workspace.RowDefinitions.Clear();
        if (compact)
        {
            ChartView.HeightRequest = 250; _codePanel.HeightRequest = 410;
            _csharpSnippet.HeightRequest = _xamlSnippet.HeightRequest = 330;
            _workspace.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            _workspace.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            _workspace.Add(_chartPanel, 0, 0); _workspace.Add(_codePanel, 0, 1);
        }
        else
        {
            ChartView.HeightRequest = 480; _codePanel.HeightRequest = 506;
            _csharpSnippet.HeightRequest = _xamlSnippet.HeightRequest = 426;
            _workspace.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            _workspace.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            _workspace.Add(_chartPanel, 0, 0); _workspace.Add(_codePanel, 1, 0);
        }
    }

    private void SelectCode(bool xaml)
    {
        _codeContent.Content = xaml ? _xamlSnippet : _csharpSnippet;
        UpdateTab(_csharpTab, !xaml); UpdateTab(_xamlTab, xaml);
    }

    private static Label MakeTab(string text, bool active)
    {
        var tab = new Label { Text = text, FontSize = 13, FontAttributes = FontAttributes.Bold, HorizontalTextAlignment = TextAlignment.Center, Padding = new Thickness(0, 10) };
        UpdateTab(tab, active); return tab;
    }

    private static void UpdateTab(Label tab, bool active)
    {
        tab.TextColor = active ? AccentIndigo : TextMuted;
        tab.BackgroundColor = active ? Color.FromArgb("#EEF2FF") : Colors.Transparent;
    }

    private static TapGestureRecognizer TapGesture(Action action)
    {
        var gesture = new TapGestureRecognizer(); gesture.Tapped += (_, _) => action(); return gesture;
    }

    private static string CreateXamlSnippet(string title) => $$"""
<!-- XAML host for {{title}}. The C# tab is the exact runtime data and renderer setup. -->
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:chartnivra="clr-namespace:ChartNivra.Maui.Controls;assembly=ChartNivra.Maui">
    <chartnivra:ChartView x:Name="chartView"
                           Title="{{title}}"
                           HeightRequest="320"
                           Theme="Light" />
</ContentPage>
""";
}
