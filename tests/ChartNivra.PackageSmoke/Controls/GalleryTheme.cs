namespace ChartNivra.Gallery.Controls;

using ChartNivra.Maui.Controls;
using ChartNivra.Core.Themes;
using Microsoft.Maui.Controls;

/// <summary>Neutral surfaces and readable text for the gallery's light and dark appearances.</summary>
internal static class GalleryTheme
{
    private static readonly Color Page = Color.FromArgb("#0B0B0C");
    private static readonly Color Surface = Color.FromArgb("#19191B");
    private static readonly Color Raised = Color.FromArgb("#232326");
    private static readonly Color Foreground = Color.FromArgb("#F4F4F5");
    private static readonly Color Muted = Color.FromArgb("#B0B0B7");

    private static string Hex(Color color) => $"#FF{(int)Math.Round(color.Red * 255):X2}{(int)Math.Round(color.Green * 255):X2}{(int)Math.Round(color.Blue * 255):X2}";

    public static void SetCodeTab(Button tab, bool selected)
    {
        tab.StyleId = "ThemeSelection";
        tab.SetAppThemeColor(Button.TextColorProperty, selected ? Color.FromArgb("#4338CA") : Color.FromArgb("#64748B"), selected ? Colors.White : Color.FromArgb("#A1A1AA"));
        tab.SetAppThemeColor(Button.BackgroundColorProperty, selected ? Colors.White : Colors.Transparent, selected ? Color.FromArgb("#525CEB") : Colors.Transparent);
        SemanticProperties.SetDescription(tab, $"{tab.Text} code{(selected ? ", selected" : "")}");
    }

    public static void SetViewSelection(ImageButton button, bool selected)
    {
        button.StyleId = "ThemeSelection";
        button.SetAppThemeColor(ImageButton.BackgroundColorProperty, selected ? Color.FromArgb("#E0E7FF") : Colors.White, selected ? Color.FromArgb("#414B88") : Color.FromArgb("#27272A"));
        button.SetAppThemeColor(ImageButton.BorderColorProperty, selected ? Color.FromArgb("#818CF8") : Color.FromArgb("#E2E8F0"), selected ? Color.FromArgb("#A5B4FC") : Color.FromArgb("#52525B"));
    }

    public static Button BackButton()
    {
        var button = new Button { StyleId = "GalleryBackButton", Text = "←   All charts", FontSize = 13, FontAttributes = FontAttributes.Bold, HeightRequest = 42, Padding = new Thickness(16, 0), CornerRadius = 21, BorderWidth = 1, HorizontalOptions = LayoutOptions.Start };
        button.SetAppThemeColor(Button.TextColorProperty, Color.FromArgb("#1E293B"), Foreground);
        button.SetAppThemeColor(Button.BackgroundColorProperty, Colors.White, Raised);
        button.SetAppThemeColor(Button.BorderColorProperty, Color.FromArgb("#DCE2E8"), Color.FromArgb("#414146"));
        button.Clicked += async (_, _) => await Shell.Current.GoToAsync("..");
        SemanticProperties.SetDescription(button, "Back to all charts");
        return button;
    }

    public static void Apply(View view)
    {
        if (view is CodeSnippetView or BrandView || view.StyleId is "GalleryBackButton" or "ThemeSelection") return;
        if (view is ChartView chart)
        {
            chart.SetAppTheme(ChartView.ThemeProperty, ChartTheme.Light, ChartTheme.Dark);
            return;
        }
        var background = view.BackgroundColor;
        if (background is not null)
        {
            var hex = Hex(background);
            var dark = hex switch
            {
                "#FFF8FAFC" or "#FFF8FAFF" => Page,
                "#FFFFFFFF" or "#FFF0F3FA" or "#FFEFF4FF" or "#FFEEF2FF" or "#FFF2F5FF" => Surface,
                "#FF172338" or "#FF182235" or "#FF1C2944" or "#FF202F45" => Surface,
                _ => null
            };
            if (dark is not null) view.SetAppThemeColor(VisualElement.BackgroundColorProperty, background, dark);
        }
        if (view is Label label && label.TextColor is { } color)
        {
            var hex = Hex(color);
            var dark = hex switch
            {
                "#FF0F172A" or "#FF0B1C30" => Foreground,
                "#FF64748B" => Muted,
                "#FF4F46E5" or "#FF4338CA" or "#FF6366F1" => Color.FromArgb("#A5B4FC"),
                _ => null
            };
            if (dark is not null) label.SetAppThemeColor(Label.TextColorProperty, color, dark);
        }
        if (view is Button button && button.TextColor is { } buttonColor)
        {
            var darkText = Hex(buttonColor) switch
            {
                "#FF0F172A" or "#FF0B1C30" => Foreground,
                "#FF64748B" => Muted,
                "#FF4F46E5" or "#FF4338CA" => Color.FromArgb("#A5B4FC"),
                _ => null
            };
            if (darkText is not null) button.SetAppThemeColor(Button.TextColorProperty, buttonColor, darkText);
        }
        if (view is Border border)
        {
            if (border.Stroke is SolidColorBrush stroke && stroke.Color is { } strokeColor && Hex(strokeColor) is "#FFE2E8F0" or "#FFE7ECF4" or "#FFC7C4D7")
                border.SetAppTheme(Border.StrokeProperty, new SolidColorBrush(strokeColor), new SolidColorBrush(Color.FromArgb("#38383D")));
            if (border.Content is View child) Apply(child);
        }
        else if (view is ContentView content && content.Content is View child) Apply(child);
        else if (view is Layout layout)
            foreach (var childView in layout.Children.OfType<View>()) Apply(childView);
    }
}

