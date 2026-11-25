namespace ChartNivra.Gallery.Controls;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;

/// <summary>
/// Minimalist side menu item with professional font icons.
/// Fixed width, no collapse/expand behavior.
/// </summary>
public class SideMenuItem : Border
{
    private readonly Label _labelControl;
    private readonly Label _iconControl;

    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(SideMenuItem),
            defaultValue: string.Empty, propertyChanged: OnTitleChanged);

    public static readonly BindableProperty IconProperty =
        BindableProperty.Create(nameof(Icon), typeof(string), typeof(SideMenuItem),
            defaultValue: string.Empty, propertyChanged: OnIconChanged);

    public static readonly BindableProperty IsSelectedProperty =
        BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(SideMenuItem),
            defaultValue: false, propertyChanged: OnSelectionChanged);

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(Command), typeof(SideMenuItem));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string Icon
    {
        get => (string)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public bool IsSelected
    {
        get => (bool)GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    public Command Command
    {
        get => (Command)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public SideMenuItem()
    {
        StrokeShape = new RoundRectangle { CornerRadius = SideMenuStyles.CornerRadius };
        HeightRequest = SideMenuStyles.MenuItemHeight;
        Padding = new Thickness(SideMenuStyles.PaddingMedium);
        BackgroundColor = Colors.Transparent;
        Stroke = Colors.Transparent;
        StrokeThickness = 0;

        _iconControl = new Label
        {
            FontFamily = "Segoe Fluent Icons, Segoe MDL2 Assets, MaterialIcons",
            FontSize = SideMenuStyles.IconSize,
            TextColor = SideMenuStyles.TextSecondary,
            VerticalOptions = LayoutOptions.Center,
            WidthRequest = SideMenuStyles.IconSize,
            HorizontalOptions = LayoutOptions.Start,
        };

        _labelControl = new Label
        {
            FontSize = SideMenuStyles.FontSizeTitle,
            TextColor = SideMenuStyles.TextSecondary,
            VerticalOptions = LayoutOptions.Center,
            Margin = new Thickness(SideMenuStyles.PaddingMedium, 0, 0, 0),
        };

        var content = new HorizontalStackLayout
        {
            Spacing = SideMenuStyles.PaddingMedium,
            VerticalOptions = LayoutOptions.Center,
        };
        content.Add(_iconControl);
        content.Add(_labelControl);

        Content = content;

        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += OnTapped;
        GestureRecognizers.Add(tapGesture);
    }

    private static void OnTitleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var item = (SideMenuItem)bindable;
        item._labelControl.Text = (string)newValue;
    }

    private static void OnIconChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var item = (SideMenuItem)bindable;
        item._iconControl.Text = GetMaterialIcon((string)newValue);
    }

    private static void OnSelectionChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var item = (SideMenuItem)bindable;
        var isSelected = (bool)newValue;

        if (isSelected)
        {
            item.BackgroundColor = SideMenuStyles.AccentSubtle;
            item.Stroke = SideMenuStyles.Accent;
            item.StrokeThickness = 1;
            item._iconControl.TextColor = SideMenuStyles.Accent;
            item._labelControl.TextColor = SideMenuStyles.TextPrimary;
            item._labelControl.FontAttributes = FontAttributes.Bold;
        }
        else
        {
            item.BackgroundColor = Colors.Transparent;
            item.Stroke = Colors.Transparent;
            item.StrokeThickness = 0;
            item._iconControl.TextColor = SideMenuStyles.TextSecondary;
            item._labelControl.TextColor = SideMenuStyles.TextSecondary;
            item._labelControl.FontAttributes = FontAttributes.None;
        }
    }

    private void OnTapped(object? sender, EventArgs e)
    {
        IsSelected = true;
        Command?.Execute(null);
    }

    private static string GetMaterialIcon(string iconName)
    {
        // Cross-platform MONOCHROME unicode characters only
        // ZERO color emojis - only professional monochrome unicode symbols
        return iconName switch
        {
            // Navigation
            "home" => "\u2302",           // House (⌂)
            "dashboard" => "\u2630",      // Trigram for heaven (☰)
            "settings" => "\u2699",       // Gear (⚙)
            
            // Charts - MONOCHROME only
            "line_chart" => "\u25B6",        // Right triangle (▶) - line
            "bar_chart" => "\u2588",         // Full block (█) - bar
            "area_chart" => "\u25B8",        // Right triangle (▸) - area
            "pie_chart" => "\u229A",         // Circled ring operator (⊚)
            "donut_chart" => "\u25CE",       // Bullseye (◎)
            "scatter_chart" => "\u2219",     // Bullet operator (∙)
            "candlestick" => "\u25B2\u25BC", // Up/down triangles (▲▼)
            "radial_gauge" => "\u23F0",      // Alarm clock (⏰)
            "radar_chart" => "\u2315",       // Telephone recorder (⌕)
            "heatmap" => "\u25A0",           // Black square (■)
            "box_plot" => "\u25A1",          // White square (□)
            
            // Financial/Trending
            "trending_up" => "\u2B06",       // Upwards arrow (⬆)
            "trending_down" => "\u2B07",     // Downwards arrow (⬇)
            
            // Real-time/Streaming
            "realtime" => "\u26A1",          // High voltage (⚡)
            "streaming" => "\u25B6",         // Play triangle (▶)
            
            // Analysis/Tools
            "annotations" => "\u270E",       // Lower right pencil (✎)
            "analysis" => "\u2588",          // Full block (█)
            "theme" => "\u26A1",             // High voltage (⚡) - monochrome
            "color_lens" => "\u25CE",        // Bullseye (◎) - lens-like
            "grid" => "\u25A1",              // White square (□)
            
            // Fallback
            _ => "\u2302",                   // Default: Home
        };
    }
}
