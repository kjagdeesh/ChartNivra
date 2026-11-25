namespace ChartNivra.Gallery.Pages;

using ChartNivra.Annotations;
using ChartNivra.Core.Charts;
using ChartNivra.Core.Primitives;
using ChartNivra.Core.Series;
using ChartNivra.Core.Themes;
using ChartNivra.Core.Transforms;
using ChartNivra.Rendering.Abstractions;
using ChartNivra.Rendering.Skia;

public class AnnotationsPage : ChartPageBase
{
    private const string Code = """
// 1. Build a line chart model
chartView.Title = "Sales with Target Line";
chartView.Theme = ChartTheme.Light;
chartView.Model.Series.Add(new LineSeriesModel
{
    Title = "Sales",
    Color = ChartColor.FromHex("#00C096"),
    StrokeWidth = 2.5f,
    ShowMarkers = true,
    MarkerRadius = 5,
    ShowDataLabels = true,
    Points = [new(0, 42), new(1, 65), new(2, 55), new(3, 88), new(4, 74),
              new(5, 95), new(6, 82), new(7, 110), new(8, 98), new(9, 120)],
});

// 2. Add a horizontal reference line annotation
var annotation = new ReferenceLineAnnotation
{
    Value        = 80,
    IsHorizontal = true,
    Color        = ChartColor.FromHex("#F59E0B"),
    Label        = "Target",
    StrokeWidth  = 1.5f,
};

// 3. Use the AnnotatedChartRenderer which wraps the base renderer
chartView.Renderer = new AnnotatedCartesianRenderer(chartView.Model,
    new List<AnnotationBase> { annotation });
""";

    public AnnotationsPage() : base("Annotations", "Reference lines and markers overlaid on a chart", Code) { }

    protected override void ConfigureChart()
    {
        ChartView.Title = "Sales with Target Line";
        ChartView.Theme = ChartTheme.Light;

        ChartView.Model.Series.Add(new LineSeriesModel
        {
            Title       = "Sales",
            Color       = ChartColor.FromHex("#00C096"),
            StrokeWidth = 2.5f,
            ShowMarkers = true,
            MarkerRadius = 5,
            ShowDataLabels = true,
            Points      = new System.Collections.Generic.List<ChartPoint>
            {
                new(0,42), new(1,65), new(2,55), new(3,88), new(4,74),
                new(5,95), new(6,82), new(7,110), new(8,98), new(9,120),
            }
        });

        var annotation = new ReferenceLineAnnotation
        {
            Value        = 80,
            IsHorizontal = true,
            Color        = ChartColor.FromHex("#F59E0B"),
            Label        = "Target",
            StrokeWidth  = 1.5f,
        };

        UseRenderer(new AnnotatedCartesianRenderer(ChartView.Model,
            new System.Collections.Generic.List<AnnotationBase> { annotation }));
    }
}
