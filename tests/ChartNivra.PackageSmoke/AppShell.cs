namespace ChartNivra.Gallery;

using ChartNivra.Gallery.Pages;
using Microsoft.Maui.Controls;

/// <summary>Dashboard-first navigation for the sample gallery.</summary>
public class AppShell : Shell
{
    public AppShell()
    {
        BackgroundColor = Colors.White;
        FlyoutBehavior = FlyoutBehavior.Disabled;

        Items.Add(new ShellContent
        {
            Title = "ChartNivra",
            Route = "dashboard",
            ContentTemplate = new DataTemplate(typeof(HomePage)),
        });

        Routing.RegisterRoute("line", typeof(LineChartPage));
        Routing.RegisterRoute("multiseries", typeof(MultiSeriesPage));
        Routing.RegisterRoute("pie", typeof(PieChartPage));
        Routing.RegisterRoute("donut", typeof(DonutChartPage));
        Routing.RegisterRoute("candlestick", typeof(CandlestickPage));
        Routing.RegisterRoute("gauge", typeof(GaugePage));
        Routing.RegisterRoute("boxplot", typeof(BoxPlotPage));
        Routing.RegisterRoute("radar", typeof(RadarChartPage));
        Routing.RegisterRoute("heatmap", typeof(HeatmapPage));
        Routing.RegisterRoute("realtime", typeof(RealTimePage));
        Routing.RegisterRoute("annotations", typeof(AnnotationsPage));
        Routing.RegisterRoute("catalog", typeof(ChartCatalogPreviewPage));
        Routing.RegisterRoute("demo-streaming", typeof(ChartNivra.RealTimeDemo.MainPage));
        Routing.RegisterRoute("demo-3d", typeof(ChartNivra.ThreeDDemo.MainPage));
    }
}
