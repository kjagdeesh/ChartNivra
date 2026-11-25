namespace ChartNivra.Gallery;

using ChartNivra.Gallery.Controls;
using ChartNivra.Gallery.Pages;
using Microsoft.Maui.Controls;

/// <summary>
/// Minimalist fixed side menu shell with professional icons.
/// Fixed 260px sidebar, no responsive collapsing.
/// </summary>
public class AppShellWithSideMenu : Shell
{
    private readonly SideMenuLayout _sideMenu;

    public AppShellWithSideMenu()
    {
        BackgroundColor = Colors.White;

        // Create side menu
        _sideMenu = new SideMenuLayout();

        // Add navigation sections to menu
        BuildMenuSections();

        // ── Standard Shell navigation setup ───────────────────────────────
        Shell.SetTabBarIsVisible(this, false);
        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);

        // ── Build Shell routes ────────────────────────────────────────────
        BuildShellRoutes();
    }

    private void BuildMenuSections()
    {
        _sideMenu.AddMenuSection("OVERVIEW",
            ("home", "Home", new Command(async () => await GoToAsync("home")))
        );

        _sideMenu.AddMenuSection("CARTESIAN CHARTS",
            ("line_chart", "Line Chart", new Command(async () => await GoToAsync("linechart"))),
            ("line_chart", "Multi-Series", new Command(async () => await GoToAsync("multiseries")))
        );

        _sideMenu.AddMenuSection("CIRCULAR CHARTS",
            ("pie_chart", "Pie Chart", new Command(async () => await GoToAsync("piechart"))),
            ("donut_chart", "Donut Chart", new Command(async () => await GoToAsync("donutchart")))
        );

        _sideMenu.AddMenuSection("FINANCIAL CHARTS",
            ("candlestick", "Candlestick", new Command(async () => await GoToAsync("candlestick")))
        );

        _sideMenu.AddMenuSection("GAUGES",
            ("radial_gauge", "Radial Gauge", new Command(async () => await GoToAsync("gauge")))
        );

        _sideMenu.AddMenuSection("STATISTICAL",
            ("box_plot", "Box Plot", new Command(async () => await GoToAsync("boxplot"))),
            ("radar_chart", "Radar Chart", new Command(async () => await GoToAsync("radar"))),
            ("heatmap", "Heatmap", new Command(async () => await GoToAsync("heatmap")))
        );

        _sideMenu.AddMenuSection("ADVANCED",
            ("realtime", "Real-Time", new Command(async () => await GoToAsync("realtime"))),
            ("annotations", "Annotations", new Command(async () => await GoToAsync("annotations")))
        );

    }

    private void BuildShellRoutes()
    {
        Routing.RegisterRoute("home", typeof(HomePage));
        Routing.RegisterRoute("linechart", typeof(LineChartPage));
        Routing.RegisterRoute("multiseries", typeof(MultiSeriesPage));
        Routing.RegisterRoute("piechart", typeof(PieChartPage));
        Routing.RegisterRoute("donutchart", typeof(DonutChartPage));
        Routing.RegisterRoute("candlestick", typeof(CandlestickPage));
        Routing.RegisterRoute("gauge", typeof(GaugePage));
        Routing.RegisterRoute("boxplot", typeof(BoxPlotPage));
        Routing.RegisterRoute("radar", typeof(RadarChartPage));
        Routing.RegisterRoute("heatmap", typeof(HeatmapPage));
        Routing.RegisterRoute("realtime", typeof(RealTimePage));
        Routing.RegisterRoute("annotations", typeof(AnnotationsPage));
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        MainThread.BeginInvokeOnMainThread(async () => 
        {
            await GoToAsync("home");
        });
    }
}
