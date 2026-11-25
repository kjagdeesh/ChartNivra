namespace ChartNivra.Gallery.Controls;

using Microsoft.Maui.Controls;

/// <summary>Shared navigation and responsive spacing for interactive studio demos.</summary>
internal static class DemoPageChrome
{
    public static void Attach(ContentPage page, VerticalStackLayout content, string title, string description)
    {
        Shell.SetNavBarIsVisible(page, false);
        page.BackgroundColor = Color.FromArgb("#F8FAFC");
        var top = new Grid { ColumnDefinitions = new ColumnDefinitionCollection { new() { Width = GridLength.Star }, new() { Width = GridLength.Auto } } };
        top.Add(new BrandView(), 0, 0);
        var back = new Button { Text = "←  All charts", FontSize = 13, HeightRequest = 42, Padding = new Thickness(14, 0), CornerRadius = 9, BackgroundColor = Colors.White, BorderColor = Color.FromArgb("#E2E8F0"), BorderWidth = 1, TextColor = Color.FromArgb("#0F172A") };
        back.Clicked += async (_, _) => await Shell.Current.GoToAsync("..");
        top.Add(back, 1, 0);
        content.Children.Insert(0, new VerticalStackLayout { Spacing = 14, Margin = new Thickness(0, 0, 0, 10), Children = { top, new BoxView { HeightRequest = 1, BackgroundColor = Color.FromArgb("#E2E8F0") }, new Label { Text = "SAMPLE LIBRARY  /  STUDIO", FontSize = 10, CharacterSpacing = 1, FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#4338CA") }, new Label { Text = title, FontSize = 28, FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#0F172A") }, new Label { Text = description, FontSize = 14, TextColor = Color.FromArgb("#64748B") } } });
        content.HorizontalOptions = LayoutOptions.Center;
        page.SizeChanged += (_, _) => { var width = page.Width; if (width <= 0) return; content.Padding = width < 600 ? new Thickness(16, 16, 16, 24) : new Thickness(24, 20, 24, 32); content.WidthRequest = width > 1440 ? 1360 : -1; };
    }
}
