namespace ChartNivra.Gallery.Pages;

using ChartNivra.Gallery.Controls;
using Microsoft.Maui.Controls;

/// <summary>Detail handoff for catalog entries whose dedicated interactive sample is next to be added.</summary>
public sealed class ChartCatalogPreviewPage : ContentPage, IQueryAttributable
{
    public ChartCatalogPreviewPage()
    {
        BackgroundColor = Color.FromArgb("#F8FAFC");
        Shell.SetNavBarIsVisible(this, true);
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var name = query.TryGetValue("name", out var rawName) ? Uri.UnescapeDataString(rawName?.ToString() ?? "Chart") : "Chart";
        var kind = query.TryGetValue("kind", out var rawKind) && Enum.TryParse(rawKind?.ToString(), out ChartThumbnailKind parsed) ? parsed : ChartThumbnailKind.Line;
        Title = name;
        Content = new VerticalStackLayout
        {
            Padding = new Thickness(20), Spacing = 18,
            Children =
            {
                new Label { Text = name, FontSize = 26, FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#0F172A") },
                new Border
                {
                    Padding = new Thickness(20), BackgroundColor = Colors.White, Stroke = Color.FromArgb("#E2E8F0"), StrokeThickness = 1,
                    Content = new ChartThumbnailView(kind) { HeightRequest = 220 },
                },
                new Label { Text = "This chart type is available in the gallery catalog. Its dedicated interactive sample is the next step in the sample app.", FontSize = 14, TextColor = Color.FromArgb("#64748B"), LineBreakMode = LineBreakMode.WordWrap },
            }
        };
    }
}
