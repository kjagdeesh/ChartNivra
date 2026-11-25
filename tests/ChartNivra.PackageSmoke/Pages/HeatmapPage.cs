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
    Title    = "Contribution activity",
    Mode     = HeatmapMode.Calendar,
    Rows     = 7,
    Columns  = 26,
    MinColor = ChartColor.FromHex("#9BE9A8"),
    MaxColor = ChartColor.FromHex("#216E39"),
    MinValue = 0,
    MaxValue = 100,
};

var rng = new Random(42);
for (int r = 0; r < 7; r++)
    for (int c = 0; c < 26; c++)
        model.Cells.Add(new HeatmapCell(r, c, rng.Next(0, 100)));

chartView.Renderer = new HeatmapRenderer(model);
""";

    public HeatmapPage() : base("Heatmap", "Color-encoded matrix heatmap", Code) { }

    protected override void ConfigureChart()
    {
        var model = new HeatmapModel
        {
            Title    = "Contribution activity",
            Mode     = HeatmapMode.Calendar,
            Rows     = 7,
            Columns  = 26,
            MinColor = ChartColor.FromHex("#9BE9A8"),
            MaxColor = ChartColor.FromHex("#216E39"),
            MinValue = 0,
            MaxValue = 100,
        };

        var rng = new System.Random(42);
        model.RowLabels.AddRange(["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"]);
        model.ColumnLabels.AddRange(["Jan", "", "", "", "Mar", "", "", "", "May", "", "", "", "Jul", "", "", "", "Sep", "", "", "", "Nov"]);
        for (int r = 0; r < 7; r++)
            for (int c = 0; c < 26; c++)
                model.Cells.Add(new HeatmapCell(r, c, rng.Next(0, 100)));

        UseRenderer(new HeatmapRenderer(model));
    }
}
