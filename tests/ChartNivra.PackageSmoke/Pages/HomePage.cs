namespace ChartNivra.Gallery.Pages;

using ChartNivra.Gallery.Controls;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;

/// <summary>A dashboard that exposes every available chart sample without a side menu.</summary>
public class HomePage : ContentPage
{
    private static readonly Color PageBackground = Color.FromArgb("#F8FAFC");
    private static readonly Color Surface = Colors.White;
    private static readonly Color Border = Color.FromArgb("#E2E8F0");
    private static readonly Color Accent = Color.FromArgb("#4F46E5");
    private static readonly Color AccentSoft = Color.FromArgb("#EEF2FF");
    private static readonly Color Text = Color.FromArgb("#0F172A");
    private static readonly Color Muted = Color.FromArgb("#64748B");

    private readonly Grid _chartGrid;
    private readonly ChartCard[] _chartCards;
    private readonly Grid _statisticsGrid;
    private readonly Border _hero;
    private readonly VerticalStackLayout _dashboardContent;
    private readonly SearchBar _search;
    private readonly Picker _familyPicker;
    private readonly Label _resultLabel;
    private bool _entrancePlayed;
    private bool _listView;
    private readonly List<Button> _familyButtons = new();

    private sealed record ChartCard(Border View, Border ListView, string Name, string Category, string Description, string[] Tags);

    public HomePage()
    {
        Title = "ChartNivra";
        BackgroundColor = PageBackground;
        this.SetAppThemeColor(BackgroundColorProperty, PageBackground, Color.FromArgb("#0B0B0C"));
        Shell.SetNavBarIsVisible(this, false);

        _hero = BuildHero(out _statisticsGrid);
        (_chartGrid, _chartCards) = BuildChartGrid();
        _search = new SearchBar
        {
            Placeholder = "Search charts, use cases, or features",
            PlaceholderColor = Muted,
            TextColor = Text,
            BackgroundColor = Surface,
            HeightRequest = 46,
        };
        _search.TextChanged += (_, _) => ApplyFilters();
        _familyPicker = new Picker { Title = "All families", TextColor = Text, TitleColor = Muted, BackgroundColor = Surface, HeightRequest = 46 };
        _familyPicker.Items.Add("All families");
        foreach (var family in _chartCards.Select(card => card.Category).Distinct().OrderBy(name => name)) _familyPicker.Items.Add(family);
        _familyPicker.SelectedIndex = 0;
        _familyPicker.SelectedIndexChanged += (_, _) => ApplyFilters();
        _resultLabel = new Label { FontSize = 12, TextColor = Muted, VerticalTextAlignment = TextAlignment.Center };
        _resultLabel.SetAppThemeColor(Label.TextColorProperty, Muted, Color.FromArgb("#AAB9CE"));
        _search.SetAppThemeColor(SearchBar.BackgroundColorProperty, Surface, Color.FromArgb("#19191B"));
        _search.SetAppThemeColor(SearchBar.TextColorProperty, Text, Color.FromArgb("#F1F5F9"));
        _familyPicker.SetAppThemeColor(Picker.BackgroundColorProperty, Surface, Color.FromArgb("#19191B"));
        _familyPicker.SetAppThemeColor(Picker.TextColorProperty, Text, Color.FromArgb("#F1F5F9"));

        var themeButton = new Button { Text = "◐  Appearance", FontSize = 13, TextColor = Text, BackgroundColor = Surface, BorderColor = Border, BorderWidth = 1, CornerRadius = 10, HorizontalOptions = LayoutOptions.End };
        themeButton.Clicked += async (_, _) =>
        {
            var choice = await DisplayActionSheet("Appearance", "Cancel", null, "Light", "Dark", "System default");
            if (Application.Current is not { } app) return;
            app.UserAppTheme = choice switch { "Light" => AppTheme.Light, "Dark" => AppTheme.Dark, "System default" => AppTheme.Unspecified, _ => app.UserAppTheme };
        };

        var topBar = new Grid { ColumnDefinitions = new ColumnDefinitionCollection { new() { Width = GridLength.Star }, new() { Width = GridLength.Auto } }, Margin = new Thickness(0, 0, 0, 24) };
        var brand = new Label { Text = "chartnivra", FontSize = 19, FontAttributes = FontAttributes.Bold, TextColor = Text, VerticalOptions = LayoutOptions.Center };
        brand.SetAppThemeColor(Label.TextColorProperty, Text, Color.FromArgb("#F1F5F9"));
        topBar.Add(new HorizontalStackLayout { Spacing = 10, Children = { new Image { Source = "chartnivra_logo.png", WidthRequest = 32, HeightRequest = 32 }, brand } }, 0, 0); topBar.Add(themeButton, 1, 0);
        Content = new ScrollView
        {
            Content = _dashboardContent = new VerticalStackLayout
            {
                Spacing = 0,
                Padding = new Thickness(24, 20, 24, 0),
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    topBar,
                    _hero,
                    BuildDiscoveryHeader(),
                    _chartGrid,
                    new Label { Text = "Choose a chart type to open its interactive sample and source preview.", FontSize = 12, TextColor = Muted, HorizontalTextAlignment = TextAlignment.Center, Margin = new Thickness(0, 0, 0, 28) },
                }
            }
        };

        GalleryTheme.Apply(_dashboardContent);
        foreach (var card in _chartCards) { GalleryTheme.Apply(card.View); GalleryTheme.Apply(card.ListView); }

        SizeChanged += (_, _) => UpdateLayoutForWidth(Width);
        ApplyFilters();
    }

    private static Border BuildHero(out Grid statisticsGrid)
    {
        statisticsGrid = new Grid { ColumnSpacing = 10, RowSpacing = 10, Margin = new Thickness(0, 4, 0, 0) };
        AddStatistics(statisticsGrid, 2);

        var hero = new Border
        {
            Padding = new Thickness(32, 28),
            BackgroundColor = Color.FromArgb("#EEF2FF"),
            StrokeThickness = 0,
            StrokeShape = new RoundRectangle { CornerRadius = 22 },
            Content = new VerticalStackLayout
            {
                Spacing = 18,
                Children =
                {
                    new Grid
                    {
                        ColumnDefinitions = new ColumnDefinitionCollection
                        {
                            new ColumnDefinition { Width = GridLength.Star },
                            new ColumnDefinition { Width = GridLength.Auto },
                        },
                        Children =
                        {
                            new VerticalStackLayout
                            {
                                Spacing = 6,
                                Children =
                                {
                                    new Label { Text = "CHARTNIVRA  /  GALLERY", FontSize = 11, FontAttributes = FontAttributes.Bold, CharacterSpacing = 1.2, TextColor = Color.FromArgb("#A5B4FC") },
                                    new Label { Text = "Explore data, beautifully.", FontSize = 28, FontAttributes = FontAttributes.Bold, TextColor = Colors.White },
                                    new Label { Text = "A practical collection of native .NET MAUI chart samples, organized by visual task.", FontSize = 14, TextColor = Color.FromArgb("#CBD5E1"), LineBreakMode = LineBreakMode.WordWrap },
                                }
                            },
                            new Border
                            {
                                Padding = new Thickness(12, 7), BackgroundColor = Color.FromArgb("#1E293B"), Stroke = Color.FromArgb("#334155"), StrokeThickness = 1,
                                StrokeShape = new RoundRectangle { CornerRadius = 14 }, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Start,
                                Content = new Label { Text = "23 TYPES", FontSize = 10, FontAttributes = FontAttributes.Bold, CharacterSpacing = .8, TextColor = Color.FromArgb("#C7D2FE") },
                            }
                        }
                    },
                }
            }
        };
        var content = (VerticalStackLayout)hero.Content;
        hero.SetAppThemeColor(VisualElement.BackgroundColorProperty, Color.FromArgb("#EEF2FF"), Color.FromArgb("#19191B"));
        var heroGrid = (Grid)content.Children[0];
        var textStack = (VerticalStackLayout)heroGrid.Children[0];
        ((Label)textStack.Children[0]).Text = "EXPLORE  ·  BUILD  ·  VISUALIZE";
        ((Label)textStack.Children[0]).TextColor = Color.FromArgb("#6366F1");
        var title = (Label)textStack.Children[1];
        title.Text = "Your data, in a new light."; title.FontSize = 34;
        title.SetAppThemeColor(Label.TextColorProperty, Text, Color.FromArgb("#F1F5F9"));
        var description = (Label)textStack.Children[2];
        description.Text = "Explore practical examples, inspect the data, and take the code with you.";
        description.SetAppThemeColor(Label.TextColorProperty, Muted, Color.FromArgb("#AAB9CE"));
        heroGrid.Children.RemoveAt(1);
        var featured = new Border
        {
            WidthRequest = 240, Padding = new Thickness(16), BackgroundColor = Colors.White,
            Stroke = Color.FromArgb("#C7C4D7"), StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 10 },
            Content = new VerticalStackLayout { Spacing = 4, Children = { new Label { Text = "FEATURED SAMPLE", FontSize = 10, FontAttributes = FontAttributes.Bold, TextColor = Accent }, new Label { Text = "Bar / Column   ↗", FontSize = 15, FontAttributes = FontAttributes.Bold, TextColor = Text }, new Label { Text = "Quarterly revenue · C# & XAML", FontSize = 11, TextColor = Muted } } }
        };
        var openFeatured = new TapGestureRecognizer();
        openFeatured.Tapped += async (_, _) => await Shell.Current.GoToAsync($"catalog?name={Uri.EscapeDataString("Bar / Column")}&kind={ChartThumbnailKind.Bar}");
        featured.GestureRecognizers.Add(openFeatured);
        heroGrid.Add(featured, 1, 0);
        heroGrid.SizeChanged += (_, _) => featured.IsVisible = heroGrid.Width >= 640;
        return hero;
    }

    private View BuildDiscoveryHeader()
    {
        var filterRow = new Grid
        {
            ColumnSpacing = 10,
            ColumnDefinitions = new ColumnDefinitionCollection { new() { Width = GridLength.Star }, new() { Width = GridLength.Auto }, new() { Width = GridLength.Auto } },
            Margin = new Thickness(0, 18, 0, 12)
        };
        filterRow.Add(new Border { Padding = new Thickness(2, 0), BackgroundColor = Surface, Stroke = Border, StrokeThickness = 1, StrokeShape = new RoundRectangle { CornerRadius = 10 }, Content = _search }, 0, 0);
        var gridButton = new ImageButton { Source = "view_grid.png", WidthRequest = 42, HeightRequest = 46, Padding = new Thickness(11), BackgroundColor = AccentSoft, CornerRadius = 8, BorderColor = Border, BorderWidth = 1 };
        var listButton = new ImageButton { Source = "view_list.png", WidthRequest = 42, HeightRequest = 46, Padding = new Thickness(11), BackgroundColor = Surface, CornerRadius = 8, BorderColor = Border, BorderWidth = 1 };
        SemanticProperties.SetDescription(gridButton, "Grid view");
        SemanticProperties.SetDescription(listButton, "List view");
        gridButton.SetAppTheme(ImageButton.SourceProperty, ImageSource.FromFile("view_grid.png"), ImageSource.FromFile("view_grid_dark.png"));
        listButton.SetAppTheme(ImageButton.SourceProperty, ImageSource.FromFile("view_list.png"), ImageSource.FromFile("view_list_dark.png"));
        GalleryTheme.SetViewSelection(gridButton, true); GalleryTheme.SetViewSelection(listButton, false);
        gridButton.Clicked += (_, _) => { _listView = false; GalleryTheme.SetViewSelection(gridButton, true); GalleryTheme.SetViewSelection(listButton, false); ApplyFilters(); };
        listButton.Clicked += (_, _) => { _listView = true; GalleryTheme.SetViewSelection(gridButton, false); GalleryTheme.SetViewSelection(listButton, true); ApplyFilters(); };
        filterRow.Add(gridButton, 1, 0); filterRow.Add(listButton, 2, 0);
        var familyRow = new FlexLayout { Direction = FlexDirection.Row, Wrap = FlexWrap.Wrap, Margin = new Thickness(0, 0, 0, 6) };
        for (var i = 0; i < _familyPicker.Items.Count; i++)
        {
            var index = i;
            var family = _familyPicker.Items[i];
            var count = index == 0 ? _chartCards.Length : _chartCards.Count(card => card.Category == family);
            var button = new Button { Text = $"{family}  {count}", FontSize = 12, Padding = new Thickness(14, 0), HeightRequest = 36, CornerRadius = 18, Margin = new Thickness(0, 0, 8, 8), BackgroundColor = index == 0 ? Accent : Surface, TextColor = index == 0 ? Colors.White : Muted, BorderColor = Border, BorderWidth = 1 };
            button.Clicked += (_, _) => { _familyPicker.SelectedIndex = index; foreach (var chip in _familyButtons) { var selected = chip == button; chip.SetAppThemeColor(Button.BackgroundColorProperty, selected ? Accent : Surface, selected ? Accent : Color.FromArgb("#19191B")); chip.SetAppThemeColor(Button.TextColorProperty, selected ? Colors.White : Muted, selected ? Colors.White : Color.FromArgb("#B0B0B7")); } };
            _familyButtons.Add(button); familyRow.Add(button);
        }
        return new VerticalStackLayout
        {
            Spacing = 5, Margin = new Thickness(0, 28, 0, 14),
            Children =
            {
                ThemedLabel("Explore the collection", 22, true),
                ThemedLabel("Find a starting point for your next visualization.", 13, false),
                filterRow,
                familyRow,
                _resultLabel,
            }
        };
    }

    private static Label ThemedLabel(string text, double size, bool heading)
    {
        var label = new Label { Text = text, FontSize = size, FontAttributes = heading ? FontAttributes.Bold : FontAttributes.None };
        label.SetAppThemeColor(Label.TextColorProperty, heading ? Text : Muted, Color.FromArgb(heading ? "#F1F5F9" : "#AAB9CE"));
        return label;
    }

    private static (Grid Grid, ChartCard[] Cards) BuildChartGrid()
    {
        var grid = new Grid { Margin = new Thickness(0, 0, 0, 24), ColumnSpacing = 14, RowSpacing = 14 };
        var samples = new (string Name, string Category, string Description, string? Route, ChartThumbnailKind Kind, string[] Tags)[]
        {
            ("Line Chart", "Cartesian", "Smooth line series and multiple data sets", "line", ChartThumbnailKind.Line, ["Spline", "Area", "Multi-series"]),
            ("Bar / Column", "Cartesian", "Grouped, stacked and horizontal bars", null, ChartThumbnailKind.Bar, ["Grouped", "Stacked", "100%"]),
            ("Area Chart", "Cartesian", "Overlapping and stacked area series", null, ChartThumbnailKind.Area, ["Stacked", "Gradient", "Multi-layer"]),
            ("Scatter / Bubble", "Cartesian", "XY points with variable-size bubbles", null, ChartThumbnailKind.Bubble, ["XY plot", "Bubble", "Clustering"]),
            ("Spline / Step", "Cartesian", "Curved and stepped line transitions", null, ChartThumbnailKind.Step, ["Bezier", "Step", "Smooth"]),
            ("Range / Band", "Cartesian", "High-low ranges and confidence bands", null, ChartThumbnailKind.Range, ["Hi-lo", "Band", "Confidence"]),
            ("Waterfall", "Cartesian", "Running totals with positive and negative changes", null, ChartThumbnailKind.Waterfall, ["Bridge", "P&L", "Running total"]),
            ("Pie Chart", "Circular", "Classic part-to-whole proportions", "pie", ChartThumbnailKind.Pie, ["Explode", "Labels", "Animated"]),
            ("Donut / Doughnut", "Circular", "Center labels and multi-ring compositions", "donut", ChartThumbnailKind.Donut, ["Ring", "Progress", "Multi-ring"]),
            ("Sunburst", "Circular", "Hierarchical drill-down segments", null, ChartThumbnailKind.Sunburst, ["Hierarchical", "Drill-down"]),
            ("Radar / Spider", "Statistical", "Multi-axis comparisons", "radar", ChartThumbnailKind.Radar, ["Spider", "Polar", "Multi-axis"]),
            ("Heatmap", "Statistical", "Calendar and matrix intensity maps", "heatmap", ChartThumbnailKind.Heatmap, ["Calendar", "Matrix", "Color scale"]),
            ("Treemap", "Hierarchical", "Proportional area hierarchy", null, ChartThumbnailKind.Treemap, ["Hierarchical", "Proportional"]),
            ("Candlestick / OHLC", "Financial", "Open, high, low and close values", "candlestick", ChartThumbnailKind.Candlestick, ["OHLC", "Volume", "Indicators"]),
            ("Technical Indicators", "Financial", "OHLCV with live moving-average overlays", null, ChartThumbnailKind.Indicator, ["SMA 5", "SMA 10", "Volume"]),
            ("Histogram", "Statistical", "Frequency distribution by bins", null, ChartThumbnailKind.Histogram, ["Frequency", "Distribution", "Bins"]),
            ("Box Plot", "Statistical", "Quartiles, outliers and variation", "boxplot", ChartThumbnailKind.BoxPlot, ["Quartile", "Outliers", "Violin"]),
            ("Radial Gauge", "Gauges", "Progress and target performance", "gauge", ChartThumbnailKind.Gauge, ["Needle", "Zones", "Speed"]),
            ("Linear / Bullet", "Gauges", "Compact target-versus-actual gauges", null, ChartThumbnailKind.Bullet, ["Bullet", "Progress", "KPI"]),
            ("Funnel / Pyramid", "Conversion", "Stage-based conversion analysis", null, ChartThumbnailKind.Funnel, ["Funnel", "Pyramid", "Conversion"]),
            ("3D Column / Bar", "3D", "Perspective columns with camera controls", null, ChartThumbnailKind.ThreeD, ["3D", "Perspective", "Orbit"]),
            ("3D Surface / Mesh", "3D", "Parametric surfaces and wireframes", null, ChartThumbnailKind.Surface, ["Mesh", "Wireframe", "Lighting"]),
            ("3D Scatter / Point Cloud", "3D", "XYZ points with depth and orbit controls", null, ChartThumbnailKind.Bubble, ["Point cloud", "XYZ", "Orbit"]),
            ("Streaming Studio", "Demos", "High-frequency data and real-time updates", "demo-streaming", ChartThumbnailKind.Line, ["Real-time"]),
            ("3D Visualization Studio", "Demos", "Explore 3D charts with camera controls", "demo-3d", ChartThumbnailKind.ThreeD, ["Camera"]),
            ("Annotations", "Demos", "Reference lines and chart markers", "annotations", ChartThumbnailKind.Line, ["Annotations"]),
            ("Real-Time Chart", "Demos", "A live updating line chart", "realtime", ChartThumbnailKind.Line, ["Live"]),
            ("Multiple Series", "Demos", "Compare multiple data series", "multiseries", ChartThumbnailKind.Line, ["Comparison"]),
        };

        var cards = new ChartCard[samples.Length];
        for (var i = 0; i < samples.Length; i++)
        {
            var (name, category, description, route, kind, tags) = samples[i];
            cards[i] = new ChartCard(MakeChartCard(name, category, description, route, kind, tags), MakeListCard(name, category, description, route, kind), name, category, description, tags);
        }

        AddChartCards(grid, cards, 2);
        return (grid, cards);
    }

    private static Border MakeListCard(string name, string category, string description, string? route, ChartThumbnailKind kind)
    {
        var preview = new Border
        {
            WidthRequest = 96, HeightRequest = 96, Padding = 6, BackgroundColor = Color.FromArgb("#F2F5FF"),
            StrokeThickness = 0, StrokeShape = new RoundRectangle { CornerRadius = 10 },
            Content = new ChartThumbnailView(kind) { WidthRequest = 84, HeightRequest = 84 }
        };
        var title = ThemedLabel(name, 16, true);
        var detail = ThemedLabel(description, 13, false);
        detail.LineBreakMode = LineBreakMode.WordWrap;
        var info = new VerticalStackLayout
        {
            Spacing = 5, VerticalOptions = LayoutOptions.Center,
            Children = { new Label { Text = category.ToUpperInvariant(), FontSize = 10, FontAttributes = FontAttributes.Bold, CharacterSpacing = 1, TextColor = Accent }, title, detail }
        };
        var row = new Grid { ColumnSpacing = 18, ColumnDefinitions = new ColumnDefinitionCollection { new() { Width = GridLength.Auto }, new() { Width = GridLength.Star }, new() { Width = GridLength.Auto } } };
        row.Add(preview, 0, 0); row.Add(info, 1, 0);
        row.Add(new Label { Text = "→", FontSize = 20, TextColor = Accent, VerticalOptions = LayoutOptions.Center }, 2, 0);
        var card = new Border { Padding = new Thickness(14), Stroke = Border, StrokeThickness = 1, StrokeShape = new RoundRectangle { CornerRadius = 14 }, Content = row };
        card.SetAppThemeColor(VisualElement.BackgroundColorProperty, Surface, Color.FromArgb("#19191B"));
        if (card.Content is View listContent) listContent.InputTransparent = true;
        var destination = route ?? $"catalog?name={Uri.EscapeDataString(name)}&kind={kind}";
        var tap = new TapGestureRecognizer();
        var navigating = false;
        tap.Tapped += async (_, _) => { if (navigating) return; navigating = true; try { await Shell.Current.GoToAsync(destination); } finally { navigating = false; } };
        card.GestureRecognizers.Add(tap);
        SemanticProperties.SetDescription(card, $"Open {name}");
        return card;
    }

    private static Border MakeChartCard(string name, string category, string description, string? route, ChartThumbnailKind kind, string[] tags)
    {
        var card = new Border
        {
            Padding = new Thickness(20),
            BackgroundColor = Surface,
            Stroke = Border,
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 16 },
            MinimumHeightRequest = 288,
            Content = new VerticalStackLayout
            {
                Spacing = 0,
                Children =
                {
                    new Border { BackgroundColor = Color.FromArgb("#F8FAFF"), StrokeThickness = 0, StrokeShape = new RoundRectangle { CornerRadius = 12 }, Padding = new Thickness(8), Margin = new Thickness(0, 0, 0, 16), Content = new ChartThumbnailView(kind) { HeightRequest = 110 } },
                    new BoxView { HeightRequest = 1, BackgroundColor = Border, Margin = new Thickness(-18, 0, -18, 14) },
                    new Label { Text = category.ToUpperInvariant(), FontSize = 10, FontAttributes = FontAttributes.Bold, CharacterSpacing = 1, TextColor = Accent, Margin = new Thickness(0, 0, 0, 7) },
                    new Label { Text = name, FontSize = 16, FontAttributes = FontAttributes.Bold, TextColor = Text, Margin = new Thickness(0, 0, 0, 6) },
                    new Label { Text = description, FontSize = 12, TextColor = Muted, LineBreakMode = LineBreakMode.WordWrap, Margin = new Thickness(0, 0, 0, 10) },
                    new Label { Text = "Explore sample  →", FontSize = 12, FontAttributes = FontAttributes.Bold, TextColor = Accent, Margin = new Thickness(0, 6, 0, 0) },
                }
            }
        };

        card.SetAppThemeColor(VisualElement.BackgroundColorProperty, Surface, Color.FromArgb("#19191B"));
        if (card.Content is View cardContent) cardContent.InputTransparent = true;
        if (card.Content is VerticalStackLayout content)
        {
            foreach (var label in content.Children.OfType<Label>())
            {
                if (label.TextColor == Text) label.SetAppThemeColor(Label.TextColorProperty, Text, Color.FromArgb("#F1F5F9"));
                else if (label.TextColor == Muted) label.SetAppThemeColor(Label.TextColorProperty, Muted, Color.FromArgb("#AAB9CE"));
            }
        }
        var tap = new TapGestureRecognizer();
        var destination = route ?? $"catalog?name={Uri.EscapeDataString(name)}&kind={kind}";
        var navigating = false;
        tap.Tapped += async (_, _) =>
        {
            if (navigating) return;
            navigating = true;
            try
            {
                await card.ScaleTo(.98, 90, Easing.CubicOut);
                await card.ScaleTo(1, 120, Easing.CubicOut);
                await Shell.Current.GoToAsync(destination);
            }
            finally { card.Scale = 1; navigating = false; }
        };
        card.GestureRecognizers.Add(tap);
        return card;
    }

    private static FlexLayout MakeTags(IEnumerable<string> tags)
    {
        var layout = new FlexLayout { Direction = FlexDirection.Row, Wrap = FlexWrap.Wrap };
        foreach (var tag in tags)
        {
            layout.Add(new Border
            {
                Padding = new Thickness(8, 3), Margin = new Thickness(0, 0, 5, 5),
                BackgroundColor = AccentSoft, Stroke = Color.FromArgb("#BAE6FD"), StrokeThickness = 1,
                StrokeShape = new RoundRectangle { CornerRadius = 9 },
                Content = new Label { Text = tag, FontSize = 10, TextColor = Accent },
            });
        }
        return layout;
    }

    private void ApplyFilters()
    {
        if (_chartCards is null || _chartCards.Length == 0 || _resultLabel is null) return;
        var search = _search.Text?.Trim() ?? string.Empty;
        var family = _familyPicker.SelectedIndex > 0 ? _familyPicker.SelectedItem?.ToString() : null;
        var results = _chartCards.Where(card =>
            (string.IsNullOrWhiteSpace(family) || card.Category == family) &&
            (string.IsNullOrWhiteSpace(search) || $"{card.Name} {card.Category} {card.Description} {string.Join(' ', card.Tags)}".Contains(search, StringComparison.OrdinalIgnoreCase))).ToArray();
        _resultLabel.Text = $"{results.Length} sample{(results.Length == 1 ? string.Empty : "s")}";
        AddChartCards(_chartGrid, results, _listView || ResponsiveLayout.IsCompact(Width) ? 1 : Width >= 1180 ? 4 : Width >= 820 ? 3 : 2, _listView);
    }

    private void UpdateLayoutForWidth(double width)
    {
        if (width <= 0) return;
        var compact = ResponsiveLayout.IsCompact(width);
        _dashboardContent.Padding = compact ? new Thickness(12, 12, 12, 0) : new Thickness(24, 20, 24, 0);
        _dashboardContent.WidthRequest = width > 1320 ? 1240 : -1;
        _hero.Padding = compact ? new Thickness(20, 22) : new Thickness(32, 28);
        ApplyFilters();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (_entrancePlayed) return;
        _entrancePlayed = true;
        _hero.Opacity = 0;
        _hero.TranslationY = 12;
        _chartGrid.Opacity = 0;
        _ = Task.WhenAll(_hero.FadeTo(1, 300, Easing.CubicOut), _hero.TranslateTo(0, 0, 300, Easing.CubicOut), _chartGrid.FadeTo(1, 400, Easing.CubicOut));
    }

    private static void AddStatistics(Grid grid, int columns)
    {
        grid.Children.Clear();
        grid.ColumnDefinitions.Clear();
        for (var i = 0; i < columns; i++) grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
        var values = new[] { ("23", "Chart types"), ("8", "Families"), ("MAUI", "Native"), ("Skia", "Rendered") };
        for (var i = 0; i < values.Length; i++) grid.Add(MakeStat(values[i].Item1, values[i].Item2), i % columns, i / columns);
    }

    private static Border MakeStat(string value, string label) => new()
    {
        Padding = new Thickness(12, 10), BackgroundColor = Color.FromArgb("#1E293B"), Stroke = Color.FromArgb("#334155"), StrokeThickness = 1,
        StrokeShape = new RoundRectangle { CornerRadius = 8 },
        Content = new VerticalStackLayout
        {
            Spacing = 1,
            Children =
            {
                new Label { Text = value, FontSize = 17, FontAttributes = FontAttributes.Bold, TextColor = Colors.White, HorizontalTextAlignment = TextAlignment.Center },
                new Label { Text = label.ToUpperInvariant(), FontSize = 10, FontAttributes = FontAttributes.Bold, CharacterSpacing = .5, TextColor = Color.FromArgb("#94A3B8"), HorizontalTextAlignment = TextAlignment.Center },
            }
        }
    };

    private static void AddChartCards(Grid grid, IEnumerable<ChartCard> cards, int columns, bool listView = false)
    {
        grid.Children.Clear();
        grid.ColumnDefinitions.Clear();
        for (var i = 0; i < columns; i++) grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
        var visibleCards = cards.ToArray();
        for (var i = 0; i < visibleCards.Length; i++) grid.Add(listView ? visibleCards[i].ListView : visibleCards[i].View, i % columns, i / columns);
    }
}
