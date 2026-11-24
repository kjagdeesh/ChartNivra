namespace ChartNivra.Gallery.Pages;

using ChartNivra.Gallery.Controls;
using ChartNivra.Cartesian;
using ChartNivra.Core.Charts;
using ChartNivra.Core.Financial;
using ChartNivra.Core.Primitives;
using ChartNivra.Core.Series;
using ChartNivra.Core.Themes;
using ChartNivra.Maui.Controls;
using ChartNivra.Pie;
using ChartNivra.Financial;
using ChartNivra.Rendering.Skia;
using ChartNivra.Statistical;
using ChartNivra.ThreeD;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;

/// <summary>Code-and-preview workspace for catalog entries without a dedicated sample page yet.</summary>
public sealed class ChartCatalogPreviewPage : ContentPage, IQueryAttributable
{
    private readonly Grid _workspace = new() { ColumnSpacing = 16, RowSpacing = 16 };
    private readonly VerticalStackLayout _content = new() { Spacing = 0, Padding = new Thickness(24, 20, 24, 28) };
    private View? _preview;
    private Border? _codePanel;
    private string _name = "Chart";
    private ChartThumbnailKind _kind;

    public ChartCatalogPreviewPage()
    {
        BackgroundColor = Color.FromArgb("#F8FAFC");
        Shell.SetNavBarIsVisible(this, true);
        _content.Add(_workspace);
        Content = new ScrollView { Content = _content };
        SizeChanged += (_, _) => UpdateLayout(Width);
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        _name = query.TryGetValue("name", out var name) ? Uri.UnescapeDataString(name?.ToString() ?? "Chart") : "Chart";
        _kind = query.TryGetValue("kind", out var kind) && Enum.TryParse(kind?.ToString(), out ChartThumbnailKind parsed) ? parsed : ChartThumbnailKind.Line;
        Title = _name;
        BuildWorkspace();
        UpdateLayout(Width);
    }

    private void BuildWorkspace()
    {
        _content.Children.Clear();
        _content.Add(new Label { Text = "CHART SAMPLE", FontSize = 10, FontAttributes = FontAttributes.Bold, CharacterSpacing = 1.2, TextColor = Color.FromArgb("#4F46E5") });
        _content.Add(new Label { Text = _name, FontSize = 28, FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#0F172A"), Margin = new Thickness(0, 3, 0, 0) });
        _content.Add(new Label { Text = "Interactive preview · complete implementation", FontSize = 14, TextColor = Color.FromArgb("#64748B"), Margin = new Thickness(0, 4, 0, 20) });
        _content.Add(_workspace);
        _preview = new Border
        {
            Padding = new Thickness(16), BackgroundColor = Colors.White, Stroke = Color.FromArgb("#E2E8F0"), StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 14 },
            Content = new VerticalStackLayout
            {
                Spacing = 8,
                Children =
                {
                    new Label { Text = "LIVE PREVIEW", FontSize = 10, FontAttributes = FontAttributes.Bold, CharacterSpacing = 1, TextColor = Color.FromArgb("#64748B") },
                    BuildChart()
                }
            }
        };
        var csharp = new CodeSnippetView { Language = CodeLanguage.CSharp, HeightRequest = 426, Code = CreateCsharpSource() };
        var xaml = new CodeSnippetView { Language = CodeLanguage.Xaml, HeightRequest = 426, Code = $"<!-- chartView is configured by the C# tab in this sample. -->\n<chartnivra:ChartView x:Name=\"chartView\"\n    Title=\"{_name}\"\n    HeightRequest=\"320\"\n    Theme=\"Light\" />" };
        var active = new ContentView { Content = csharp };
        var csTab = MakeTab("C#", true); var xamlTab = MakeTab("XAML", false);
        csTab.GestureRecognizers.Add(Tap(() => { active.Content = csharp; SetTab(csTab, true); SetTab(xamlTab, false); }));
        xamlTab.GestureRecognizers.Add(Tap(() => { active.Content = xaml; SetTab(csTab, false); SetTab(xamlTab, true); }));
        var tabs = new Grid { ColumnDefinitions = new ColumnDefinitionCollection { new() { Width = GridLength.Star }, new() { Width = GridLength.Star } } };
        tabs.Add(csTab, 0, 0); tabs.Add(xamlTab, 1, 0);
        _codePanel = new Border
        {
            BackgroundColor = Colors.White, Stroke = Color.FromArgb("#E2E8F0"), StrokeThickness = 1, StrokeShape = new RoundRectangle { CornerRadius = 14 },
            Content = new VerticalStackLayout { Spacing = 0, Children = { new Label { Text = "IMPLEMENTATION", FontSize = 10, FontAttributes = FontAttributes.Bold, CharacterSpacing = 1, TextColor = Color.FromArgb("#64748B"), Padding = new Thickness(16, 14, 16, 4) }, tabs, new BoxView { HeightRequest = 1, BackgroundColor = Color.FromArgb("#E2E8F0") }, active } }
        };
    }

    private View BuildChart()
    {
        var chart = new ChartView { HeightRequest = 360, Theme = ChartTheme.Light, Title = _name };
        switch (_kind)
        {
            case ChartThumbnailKind.Bar:
                var bar = new BarChartModel { Title = _name, Theme = ChartTheme.Light, ShowValueLabels = true };
                bar.Categories.AddRange(["Q1", "Q2", "Q3", "Q4"]);
                bar.Series.Add(new BarSeriesModel { Title = "Revenue", Color = ChartColor.FromHex("#38BDF8"), Points = [new(0, 42), new(1, 68), new(2, 57), new(3, 84)] });
                chart.Renderer = new BarChartRenderer(bar); break;
            case ChartThumbnailKind.Bubble:
                if (_name.StartsWith("3D Scatter", StringComparison.Ordinal))
                {
                    var scatter = new ScatterChart3DModel { Title = _name };
                    scatter.Points.AddRange([new(-3, 1, -2, 7, ChartColor.FromHex("#38BDF8"), "North"), new(-1, 3, 1, 9, ChartColor.FromHex("#818CF8"), "East"), new(1, 2, -1, 8, ChartColor.FromHex("#22C55E"), "Core"), new(3, 4, 2, 10, ChartColor.FromHex("#F97316"), "West"), new(0, 5, 0, 7, ChartColor.FromHex("#EC4899"), "Peak")]);
                    chart.Renderer = new ScatterChart3DRenderer(scatter);
                }
                else
                {
                    var bubble = new BubbleSeriesModel(_name, ChartColor.FromHex("#38BDF8"));
                    bubble.Points.AddRange([new(10, 40, 12, "A"), new(24, 65, 22, "B"), new(42, 52, 16, "C"), new(58, 82, 28, "D")]);
                    chart.Renderer = new BubbleChartRenderer(bubble);
                }
                break;
            case ChartThumbnailKind.Range:
                var range = new RangeSeriesModel(_name, ChartColor.FromHex("#818CF8"));
                range.Points.AddRange([new(0, 24, 48), new(1, 32, 62), new(2, 28, 58), new(3, 42, 74), new(4, 38, 69)]);
                chart.Renderer = new RangeAreaRenderer(range); break;
            case ChartThumbnailKind.Waterfall:
                var waterfall = new WaterfallSeriesModel(_name);
                waterfall.Points.AddRange([new("Start", 120, WaterfallType.Total), new("Sales", 48), new("Costs", -31, WaterfallType.Decremental), new("End", 0, WaterfallType.Total)]);
                chart.Renderer = new WaterfallChartRenderer(waterfall); break;
            case ChartThumbnailKind.Histogram:
                var histogram = new HistogramModel { Title = _name, BinCount = 8 };
                histogram.Data.AddRange([12d, 18, 21, 25, 28, 29, 32, 35, 37, 39, 42, 48, 52, 56, 61, 64, 69, 72]);
                chart.Renderer = new HistogramRenderer(histogram); break;
            case ChartThumbnailKind.Funnel:
                var funnel = new FunnelSeriesModel { Title = _name };
                funnel.Stages.AddRange([new("Visitors", 10000), new("Leads", 6500), new("Trials", 3200), new("Sales", 980)]);
                chart.Renderer = new FunnelChartRenderer(funnel); break;
            case ChartThumbnailKind.Treemap:
                var tree = new HierarchyNode("Revenue"); tree.Children.AddRange([new("Mobile", 48, ChartColor.FromHex("#38BDF8")), new("Web", 31, ChartColor.FromHex("#818CF8")), new("Retail", 21, ChartColor.FromHex("#22C55E"))]);
                chart.Renderer = new TreemapRenderer(new TreemapModel(_name, tree)); break;
            case ChartThumbnailKind.Sunburst:
                var root = new HierarchyNode("All");
                var product = new HierarchyNode("Product", 55, ChartColor.FromHex("#38BDF8")); product.Children.AddRange([new("Core", 35), new("Plus", 20)]);
                var service = new HierarchyNode("Service", 45, ChartColor.FromHex("#818CF8")); service.Children.AddRange([new("Consulting", 25), new("Support", 20)]);
                root.Children.AddRange([product, service]);
                chart.Renderer = new SunburstRenderer(new SunburstModel(_name, root)); break;
            case ChartThumbnailKind.Area:
                chart.Model.Series.Add(new LineSeriesModel { Title = _name, Color = ChartColor.FromHex("#818CF8"), Smooth = _kind == ChartThumbnailKind.Area, ShowArea = _kind == ChartThumbnailKind.Area, AreaOpacity = 90, Points = [new(0, 20), new(1, 44), new(2, 36), new(3, 68), new(4, 56), new(5, 82)] }); break;
            case ChartThumbnailKind.Step:
                var step = new StepLineSeriesModel(_name, ChartColor.FromHex("#F97316")) { Mode = StepMode.After, Points = [new(0, 24), new(1, 48), new(2, 42), new(3, 68), new(4, 60), new(5, 84)] };
                chart.Renderer = new StepLineRenderer(step); break;
            case ChartThumbnailKind.Indicator:
                chart.Renderer = new FinancialChartRenderer(CreateTechnicalModel()); break;
            case ChartThumbnailKind.Bullet:
                chart.Renderer = new BulletChartRenderer(new BulletChartModel { Title = _name, Value = 68, Target = 75, Maximum = 100, QualitativeRanges = [45, 70, 100] }); break;
            case ChartThumbnailKind.ThreeD:
            case ChartThumbnailKind.Surface:
                if (_kind == ChartThumbnailKind.Surface || _name.StartsWith("3D Surface", StringComparison.Ordinal))
                {
                    var surface = new SurfaceChart3DModel { Title = _name, ResolutionX = 18, ResolutionY = 18, MinX = -4, MaxX = 4, MinY = -4, MaxY = 4, HeightFunction = (x, y) => Math.Sin(x) * Math.Cos(y) * 2.4 };
                    chart.Renderer = new SurfaceChart3DRenderer(surface);
                }
                else
                {
                    var columns = new ColumnChart3DModel { Title = _name, XAxisTitle = "Quarter", YAxisTitle = "Revenue (M)", ZAxisTitle = "Region" };
                    columns.AddPoint(-2.5, 3.5, -1.5, ChartColor.FromHex("#38BDF8"), "Q1"); columns.AddPoint(-0.8, 5.5, 0, ChartColor.FromHex("#818CF8"), "Q2"); columns.AddPoint(0.9, 4.2, -0.5, ChartColor.FromHex("#22C55E"), "Q3"); columns.AddPoint(2.6, 6.7, 1.2, ChartColor.FromHex("#F97316"), "Q4");
                    chart.Renderer = new ColumnChart3DRenderer(columns);
                }
                break;
            default:
                chart.Model.Series.Add(new LineSeriesModel { Title = _name, Color = ChartColor.FromHex("#38BDF8"), Points = [new(0, 22), new(1, 48), new(2, 37), new(3, 71), new(4, 60), new(5, 88)] }); break;
        }
        return chart;
    }

    private FinancialChartModel CreateTechnicalModel()
    {
        var model = new FinancialChartModel { Title = _name, ShowVolume = true };
        var start = new DateTime(2025, 1, 2);
        var close = 102d;
        for (var i = 0; i < 28; i++)
        {
            var open = close;
            close = Math.Round(open + Math.Sin(i * .74) * 2.3 + (i % 5 - 2) * .38, 2);
            model.Candlesticks.Data.Add(new FinancialPoint(start.AddDays(i), open, Math.Max(open, close) + 1.6, Math.Min(open, close) - 1.4, close, 900 + i * 45));
        }
        model.AddSmaOverlay(5, ChartColor.FromHex("#38BDF8"));
        model.AddSmaOverlay(10, ChartColor.FromHex("#F97316"));
        return model;
    }

    private string CreateCsharpSource()
    {
        if (_kind == ChartThumbnailKind.Bar) return "var model = new BarChartModel { Title = \"Quarterly Revenue\", Theme = ChartTheme.Light, ShowValueLabels = true };\nmodel.Categories.AddRange([\"Q1\", \"Q2\", \"Q3\", \"Q4\"]);\nmodel.Series.Add(new BarSeriesModel { Title = \"Revenue\", Color = ChartColor.FromHex(\"#38BDF8\"), Points = [new(0, 42), new(1, 68), new(2, 57), new(3, 84)] });\nchartView.Renderer = new BarChartRenderer(model);";
        if (_kind == ChartThumbnailKind.Bubble) return _name.StartsWith("3D Scatter", StringComparison.Ordinal)
            ? "var model = new ScatterChart3DModel { Title = \"Point cloud\", XAxisTitle = \"X\", YAxisTitle = \"Y\", ZAxisTitle = \"Z\" };\nmodel.Points.AddRange([new(-3, 1, -2, 7, null, \"North\"), new(-1, 3, 1, 9, null, \"East\"), new(1, 2, -1, 8, null, \"Core\"), new(3, 4, 2, 10, null, \"West\"), new(0, 5, 0, 7, null, \"Peak\")]);\nchartView.Renderer = new ScatterChart3DRenderer(model);"
            : "var model = new BubbleSeriesModel(\"Market\", ChartColor.FromHex(\"#38BDF8\"));\nmodel.Points.AddRange([new(10, 40, 12, \"A\"), new(24, 65, 22, \"B\"), new(42, 52, 16, \"C\"), new(58, 82, 28, \"D\")]);\nchartView.Renderer = new BubbleChartRenderer(model);";
        if (_kind == ChartThumbnailKind.Histogram) return "var model = new HistogramModel { Title = \"Distribution\", BinCount = 8 };\nmodel.Data.AddRange([12d, 18, 21, 25, 28, 29, 32, 35, 37, 39, 42, 48, 52, 56, 61, 64, 69, 72]);\nchartView.Renderer = new HistogramRenderer(model);";
        if (_kind == ChartThumbnailKind.Area) return "chartView.Model.Series.Add(new LineSeriesModel\n{\n    Title = \"Area\", Color = ChartColor.FromHex(\"#818CF8\"), Smooth = true, ShowArea = true, AreaOpacity = 90,\n    Points = [new(0, 20), new(1, 44), new(2, 36), new(3, 68), new(4, 56), new(5, 82)]\n});";
        if (_kind == ChartThumbnailKind.Step) return "var model = new StepLineSeriesModel(\"Step line\", ChartColor.FromHex(\"#F97316\"))\n{\n    Mode = StepMode.After,\n    Points = [new(0, 24), new(1, 48), new(2, 42), new(3, 68), new(4, 60), new(5, 84)]\n};\nchartView.Renderer = new StepLineRenderer(model);";
        if (_kind == ChartThumbnailKind.Range) return "var model = new RangeSeriesModel(\"Confidence band\", ChartColor.FromHex(\"#818CF8\"));\nmodel.Points.AddRange([new(0, 24, 48), new(1, 32, 62), new(2, 28, 58), new(3, 42, 74), new(4, 38, 69)]);\nchartView.Renderer = new RangeAreaRenderer(model);";
        if (_kind == ChartThumbnailKind.Waterfall) return "var model = new WaterfallSeriesModel(\"Revenue bridge\");\nmodel.Points.AddRange([new(\"Start\", 120, WaterfallType.Total), new(\"Sales\", 48), new(\"Costs\", -31, WaterfallType.Decremental), new(\"End\", 0, WaterfallType.Total)]);\nchartView.Renderer = new WaterfallChartRenderer(model);";
        if (_kind == ChartThumbnailKind.Funnel) return "var model = new FunnelSeriesModel { Title = \"Conversion funnel\" };\nmodel.Stages.AddRange([new(\"Visitors\", 10000), new(\"Leads\", 6500), new(\"Trials\", 3200), new(\"Sales\", 980)]);\nchartView.Renderer = new FunnelChartRenderer(model);";
        if (_kind == ChartThumbnailKind.Bullet) return "var model = new BulletChartModel\n{\n    Title = \"KPI attainment\", Value = 68, Target = 75, Maximum = 100,\n    QualitativeRanges = [45, 70, 100]\n};\nchartView.Renderer = new BulletChartRenderer(model);";
        if (_kind == ChartThumbnailKind.Indicator) return "var model = new FinancialChartModel { Title = \"Technical indicators\", ShowVolume = true };\nvar start = new DateTime(2025, 1, 2);\nvar close = 102d;\nfor (var i = 0; i < 28; i++)\n{\n    var open = close;\n    close = Math.Round(open + Math.Sin(i * .74) * 2.3 + (i % 5 - 2) * .38, 2);\n    model.Candlesticks.Data.Add(new FinancialPoint(start.AddDays(i), open, Math.Max(open, close) + 1.6, Math.Min(open, close) - 1.4, close, 900 + i * 45));\n}\nmodel.AddSmaOverlay(5, ChartColor.FromHex(\"#38BDF8\"));\nmodel.AddSmaOverlay(10, ChartColor.FromHex(\"#F97316\"));\nchartView.Renderer = new FinancialChartRenderer(model);";
        if (_kind == ChartThumbnailKind.Treemap) return "var root = new HierarchyNode(\"Revenue\");\nroot.Children.AddRange([new(\"Mobile\", 48, ChartColor.FromHex(\"#38BDF8\")), new(\"Web\", 31, ChartColor.FromHex(\"#818CF8\")), new(\"Retail\", 21, ChartColor.FromHex(\"#22C55E\"))]);\nchartView.Renderer = new TreemapRenderer(new TreemapModel(\"Revenue\", root));";
        if (_kind == ChartThumbnailKind.Sunburst) return "var root = new HierarchyNode(\"All\");\nvar product = new HierarchyNode(\"Product\", 55, ChartColor.FromHex(\"#38BDF8\")); product.Children.AddRange([new(\"Core\", 35), new(\"Plus\", 20)]);\nvar service = new HierarchyNode(\"Service\", 45, ChartColor.FromHex(\"#818CF8\")); service.Children.AddRange([new(\"Consulting\", 25), new(\"Support\", 20)]);\nroot.Children.AddRange([product, service]);\nchartView.Renderer = new SunburstRenderer(new SunburstModel(\"Portfolio\", root));";
        if (_kind is ChartThumbnailKind.ThreeD or ChartThumbnailKind.Surface) return (_kind == ChartThumbnailKind.Surface || _name.StartsWith("3D Surface", StringComparison.Ordinal))
            ? "var model = new SurfaceChart3DModel { Title = \"Surface mesh\", ResolutionX = 18, ResolutionY = 18, MinX = -4, MaxX = 4, MinY = -4, MaxY = 4, HeightFunction = (x, y) => Math.Sin(x) * Math.Cos(y) * 2.4 };\nchartView.Renderer = new SurfaceChart3DRenderer(model);"
            : _name.StartsWith("3D Scatter", StringComparison.Ordinal)
                ? "var model = new ScatterChart3DModel { Title = \"Point cloud\", XAxisTitle = \"X\", YAxisTitle = \"Y\", ZAxisTitle = \"Z\" };\nmodel.Points.AddRange([new(-3, 1, -2, 7, null, \"North\"), new(-1, 3, 1, 9, null, \"East\"), new(1, 2, -1, 8, null, \"Core\"), new(3, 4, 2, 10, null, \"West\"), new(0, 5, 0, 7, null, \"Peak\")]);\nchartView.Renderer = new ScatterChart3DRenderer(model);"
                : "var model = new ColumnChart3DModel { Title = \"3D revenue\", XAxisTitle = \"Quarter\", YAxisTitle = \"Revenue (M)\", ZAxisTitle = \"Region\" };\nmodel.AddPoint(-2.5, 3.5, -1.5, ChartColor.FromHex(\"#38BDF8\"), \"Q1\"); model.AddPoint(-0.8, 5.5, 0, ChartColor.FromHex(\"#818CF8\"), \"Q2\");\nmodel.AddPoint(0.9, 4.2, -0.5, ChartColor.FromHex(\"#22C55E\"), \"Q3\"); model.AddPoint(2.6, 6.7, 1.2, ChartColor.FromHex(\"#F97316\"), \"Q4\");\nchartView.Renderer = new ColumnChart3DRenderer(model);";
        return "chartView.Model.Series.Add(new LineSeriesModel { Title = \"Sample\", Color = ChartColor.FromHex(\"#38BDF8\"), Points = [new(0, 22), new(1, 48), new(2, 37), new(3, 71), new(4, 60), new(5, 88)] });";
    }

    private void UpdateLayout(double width)
    {
        if (width <= 0 || _preview is null || _codePanel is null) return;
        var compact = ResponsiveLayout.IsCompact(width);
        _content.Padding = compact ? new Thickness(12, 16, 12, 20) : new Thickness(24, 20, 24, 28);
        _workspace.Children.Clear(); _workspace.ColumnDefinitions.Clear(); _workspace.RowDefinitions.Clear();
        if (compact) { _workspace.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); _workspace.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); _workspace.Add(_preview, 0, 0); _workspace.Add(_codePanel, 0, 1); }
        else { _workspace.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); _workspace.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); _workspace.Add(_preview, 0, 0); _workspace.Add(_codePanel, 1, 0); }
    }

    private static Label MakeTab(string text, bool active) { var tab = new Label { Text = text, FontSize = 13, FontAttributes = FontAttributes.Bold, HorizontalTextAlignment = TextAlignment.Center, Padding = new Thickness(0, 10) }; SetTab(tab, active); return tab; }
    private static void SetTab(Label tab, bool active) { tab.TextColor = active ? Color.FromArgb("#4F46E5") : Color.FromArgb("#64748B"); tab.BackgroundColor = active ? Color.FromArgb("#EEF2FF") : Colors.Transparent; }
    private static TapGestureRecognizer Tap(Action action) { var gesture = new TapGestureRecognizer(); gesture.Tapped += (_, _) => action(); return gesture; }
}
