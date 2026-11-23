namespace ChartNivra.Gallery.Pages;

using ChartNivra.Core.Primitives;
using ChartNivra.Core.Themes;
using ChartNivra.Rendering.Skia;
using ChartNivra.Statistical;

public class HeatmapPage : ChartPageBase
{
    private const string Code = """
var model = new HeatmapModel
{
    Title    = "Activity Heatmap",
    Rows     = 7,
    Columns  = 12,
    MinColor = ChartColor.FromHex("#E0F2FE"),
    MaxColor = ChartColor.FromHex("#0284C7"),
    MinValue = 0,
    MaxValue = 100,
};

var rng = new Random(42);
for (int r = 0; r < 7; r++)
    for (int c = 0; c < 12; c++)
        model.Cells.Add(new HeatmapCell(r, c, rng.Next(0, 100)));

chartView.Renderer = new HeatmapRenderer(model);
""";

    public HeatmapPage() : base("Heatmap", "Color-encoded matrix heatmap", Code) { }

    protected override void ConfigureChart()
    {
        var model = new HeatmapModel
        {
            Title    = "Activity Heatmap",
            Rows     = 7,
            Columns  = 12,
            MinColor = ChartColor.FromHex("#E0F2FE"),
            MaxColor = ChartColor.FromHex("#0284C7"),
            MinValue = 0,
            MaxValue = 100,
        };

        var rng = new System.Random(42);
        for (int r = 0; r < 7; r++)
            for (int c = 0; c < 12; c++)
                model.Cells.Add(new HeatmapCell(r, c, rng.Next(0, 100)));

        UseRenderer(new HeatmapRenderer(model));
    }
}
