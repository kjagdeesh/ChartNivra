namespace ChartNivra.Gallery.Controls;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;

/// <summary>Fixed minimalist side menu layout - always visible, no responsive collapsing.</summary>
public class SideMenuLayout : Grid
{
    private readonly VerticalStackLayout _menuContainer;
    private readonly ScrollView _scrollView;

    public SideMenuLayout()
    {
        BackgroundColor = SideMenuStyles.Background;
        Padding = 0;
        RowSpacing = 0;
        ColumnSpacing = 0;

        ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(SideMenuStyles.SidebarWidth, GridUnitType.Absolute) });

        var header = BuildHeader();

        _menuContainer = new VerticalStackLayout
        {
            Spacing = 0,
            Padding = new Thickness(0, SideMenuStyles.PaddingSmall, 0, SideMenuStyles.PaddingSmall),
        };

        _scrollView = new ScrollView
        {
            Content = _menuContainer,
            VerticalScrollBarVisibility = ScrollBarVisibility.Never,
        };

        var footer = BuildFooter();

        RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
        RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        this.SetRow(header, 0);
        this.SetColumn(header, 0);
        this.Add(header);

        this.SetRow(_scrollView, 1);
        this.SetColumn(_scrollView, 0);
        this.Add(_scrollView);

        this.SetRow(footer, 2);
        this.SetColumn(footer, 0);
        this.Add(footer);
    }

    private View BuildHeader()
    {
        var container = new VerticalStackLayout
        {
            Padding = new Thickness(SideMenuStyles.PaddingLarge, SideMenuStyles.PaddingLarge, 
                SideMenuStyles.PaddingLarge, SideMenuStyles.PaddingMedium),
            Spacing = SideMenuStyles.PaddingSmall,
            BackgroundColor = Colors.White,
        };

        // Clean logo with professional icon on white background
        var logoContainer = new Grid
        {
            HeightRequest = 40,
            WidthRequest = 40,
            HorizontalOptions = LayoutOptions.Start,
        };
        logoContainer.Add(new Label
        {
            FontFamily = "Segoe Fluent Icons, Segoe MDL2 Assets, MaterialIcons",
            Text = "\uE9F3",  // Line chart icon
            FontSize = 24,
            TextColor = SideMenuStyles.Accent,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,
        }, 0, 0);
        logoContainer.Add(new Border
        {
            StrokeShape = new RoundRectangle { CornerRadius = 8 },
            Stroke = SideMenuStyles.Accent,
            StrokeThickness = 2,
            BackgroundColor = Colors.Transparent,
            HeightRequest = 40,
            WidthRequest = 40,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,
        }, 0, 0);

        var title = new Label
        {
            Text = "ChartNivra",
            FontSize = 16,
            FontAttributes = FontAttributes.Bold,
            TextColor = SideMenuStyles.TextPrimary,
        };

        var subtitle = new Label
        {
            Text = "Gallery",
            FontSize = SideMenuStyles.FontSizeCaption,
            TextColor = SideMenuStyles.TextMuted,
        };

        container.Add(logoContainer);
        container.Add(title);
        container.Add(subtitle);

        var divider = new BoxView
        {
            HeightRequest = 1,
            BackgroundColor = SideMenuStyles.BorderSubtle,
            Margin = new Thickness(0, SideMenuStyles.PaddingSmall, 0, 0),
        };

        var wrapper = new VerticalStackLayout { Spacing = 0, BackgroundColor = Colors.White };
        wrapper.Add(container);
        wrapper.Add(divider);
        return wrapper;
    }

    private View BuildFooter()
    {
        return new Label
        {
            Text = "ChartNivra v1.0",
            FontSize = SideMenuStyles.FontSizeCaption,
            TextColor = SideMenuStyles.TextMuted,
            HorizontalOptions = LayoutOptions.Center,
            Padding = new Thickness(0, SideMenuStyles.PaddingMedium, 0, SideMenuStyles.PaddingLarge),
        };
    }

    public void AddMenuSection(string title, params (string icon, string label, Command command)[] items)
    {
        var header = new Label
        {
            Text = title.ToUpper(),
            FontSize = SideMenuStyles.FontSizeHeader,
            FontAttributes = FontAttributes.Bold,
            TextColor = SideMenuStyles.TextMuted,
            Padding = new Thickness(SideMenuStyles.PaddingMedium, SideMenuStyles.SpacingSections, 
                SideMenuStyles.PaddingMedium, SideMenuStyles.PaddingSmall),
        };
        _menuContainer.Add(header);

        foreach (var (icon, label, command) in items)
        {
            var item = new SideMenuItem
            {
                Icon = icon,
                Title = label,
                Command = command,
                HeightRequest = SideMenuStyles.MenuItemHeight,
            };
            _menuContainer.Add(item);
        }

        var divider = new BoxView
        {
            HeightRequest = 1,
            BackgroundColor = SideMenuStyles.BorderSubtle,
            Margin = new Thickness(SideMenuStyles.PaddingSmall, SideMenuStyles.SpacingItems, SideMenuStyles.PaddingSmall, 0),
        };
        _menuContainer.Add(divider);
    }
}