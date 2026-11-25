namespace ChartNivra.UnitTests;

using ChartNivra.Core.Circular;
using ChartNivra.Core.Primitives;
using ChartNivra.Core.ThreeD;
using Xunit;

public class Phase2Tests
{
    [Fact]
    public void PieChartModel_CalculatesTotalAccurately()
    {
        var model = new PieChartModel();
        model.Slices.Add(new PieSliceModel { Label = "A", Value = 30 });
        model.Slices.Add(new PieSliceModel { Label = "B", Value = 70 });

        Assert.Equal(100, model.TotalValue);
    }

    [Fact]
    public void Camera3D_ProjectsPointToScreenViewport()
    {
        var camera = new Camera3D();
        var viewport = new ChartRect(0, 0, 800, 600);

        var screenPt = camera.Project(new Vector3D(0, 0, 0), viewport);

        // Target (0, 0, 0) should project near the center of the viewport
        Assert.False(screenPt.IsEmpty);
        Assert.InRange(screenPt.X, 350, 450);
        Assert.InRange(screenPt.Y, 250, 350);
    }
}
