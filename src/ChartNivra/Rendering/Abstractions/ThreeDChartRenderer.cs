namespace ChartNivra.Rendering.Abstractions;

using ChartNivra.Core.Primitives;
using ChartNivra.Core.Themes;
using ChartNivra.Core.ThreeD;
using System.Numerics;

/// <summary>
/// Renders 3D geometry onto an IChartCanvas using painter's depth sorting and lighting.
/// </summary>
public class ThreeDChartRenderer : IChartRenderer
{
    public Camera3D Camera { get; set; } = new();
    public Light3D Light { get; set; } = new();
    public ChartTheme Theme { get; set; } = ChartTheme.Dark;
    public List<Triangle3D> Meshes { get; } = new();

    public void Render(IChartCanvas canvas, ChartRect viewport)
    {
        // 1. Draw 3D Viewport Background
        canvas.DrawRectangle(viewport, new ChartBrush(Theme.BackgroundColor));

        if (Meshes.Count == 0) return;

        // 2. Sort triangles by depth (Painter's algorithm: back-to-front)
        var viewMatrix = Camera.GetViewMatrix();
        var sorted = Meshes
            .Select(t => (Triangle: t, Depth: t.AverageDepth(viewMatrix)))
            .OrderBy(x => x.Depth)
            .Select(x => x.Triangle)
            .ToList();

        // 3. Project and draw shaded triangles
        foreach (var tri in sorted)
        {
            var p0 = Camera.Project(new Vector3D(tri.V0.X, tri.V0.Y, tri.V0.Z), viewport);
            var p1 = Camera.Project(new Vector3D(tri.V1.X, tri.V1.Y, tri.V1.Z), viewport);
            var p2 = Camera.Project(new Vector3D(tri.V2.X, tri.V2.Y, tri.V2.Z), viewport);

            if (p0.IsEmpty || p1.IsEmpty || p2.IsEmpty) continue;

            // Apply directional lighting
            var shadedColor = Light.ApplyLighting(tri.Color, tri.Normal);
            var brush = new ChartBrush(shadedColor);
            var edgePen = new ChartPen(shadedColor.WithAlpha(180), 1f);

            var poly = new List<ChartPoint> { p0, p1, p2, p0 };
            canvas.DrawPolygon(poly, brush, edgePen);
        }
    }
}
