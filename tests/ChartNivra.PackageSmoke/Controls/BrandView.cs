namespace ChartNivra.Gallery.Controls;

using Microsoft.Maui.Controls;

internal sealed class BrandView : HorizontalStackLayout
{
    public BrandView()
    {
        Spacing = 9;
        VerticalOptions = LayoutOptions.Center;
        Children.Add(new Image { Source = "chartnivra_logo.png", WidthRequest = 30, HeightRequest = 30 });
        var label = new Label { Text = "chartnivra", FontSize = 17, FontAttributes = FontAttributes.Bold, VerticalOptions = LayoutOptions.Center };
        label.SetAppThemeColor(Label.TextColorProperty, Color.FromArgb("#0F172A"), Color.FromArgb("#F1F5F9"));
        Children.Add(label);
    }
}
