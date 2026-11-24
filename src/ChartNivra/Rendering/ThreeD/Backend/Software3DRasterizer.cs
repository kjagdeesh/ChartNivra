namespace ChartNivra.Rendering.ThreeD.Backend;

using System.Numerics;
using ChartNivra.Core.Primitives;
using ChartNivra.Core.ThreeD;
using ChartNivra.Rendering.Abstractions;
using ChartNivra.Rendering.ThreeD.Abstractions;

/// <summary>
/// In-memory geometry buffer implementation.
/// </summary>
public class GeometryBuffer3D : IGeometryBuffer3D
{
    private readonly List<Vector3D> _vertices = new();
    private readonly List<Vector3D> _normals = new();
    private readonly List<int> _indices = new();

    public IReadOnlyList<Vector3D> Vertices => _vertices;
    public IReadOnlyList<Vector3D> Normals => _normals;
    public IReadOnlyList<int> Indices => _indices;

    public void AddVertex(Vector3D vertex, Vector3D normal)
    {
        _vertices.Add(vertex);
        _normals.Add(normal);
    }

    public void AddTriangle(int i0, int i1, int i2)
    {
        _indices.Add(i0);
        _indices.Add(i1);
        _indices.Add(i2);
    }

    public void Clear()
    {
        _vertices.Clear();
        _normals.Clear();
        _indices.Clear();
    }
}

/// <summary>
/// Software 3D rasterizer implementing Painter's algorithm with Lambertian shading.
/// </summary>
public class Software3DRasterizer
{
    private readonly Light3D _light = new();

    public void Render(IChartCanvas canvas, IGeometryBuffer3D buffer, Camera3D camera, ChartRect viewport, ChartColor baseColor)
    {
        if (buffer.Indices.Count < 3) return;

        var triangles = new List<Triangle3D>();
        var viewMatrix = camera.GetViewMatrix();

        for (var i = 0; i < buffer.Indices.Count; i += 3)
        {
            var v0 = buffer.Vertices[buffer.Indices[i]];
            var v1 = buffer.Vertices[buffer.Indices[i + 1]];
            var v2 = buffer.Vertices[buffer.Indices[i + 2]];

            var vec0 = new Vector3((float)v0.X, (float)v0.Y, (float)v0.Z);
            var vec1 = new Vector3((float)v1.X, (float)v1.Y, (float)v1.Z);
            var vec2 = new Vector3((float)v2.X, (float)v2.Y, (float)v2.Z);

            triangles.Add(new Triangle3D(vec0, vec1, vec2, baseColor));
        }

        // Painter's algorithm: sort triangles back-to-front by depth relative to camera
        triangles.Sort((a, b) => b.AverageDepth(viewMatrix).CompareTo(a.AverageDepth(viewMatrix)));

        foreach (var tri in triangles)
        {
            var p0 = camera.Project(new Vector3D(tri.V0.X, tri.V0.Y, tri.V0.Z), viewport);
            var p1 = camera.Project(new Vector3D(tri.V1.X, tri.V1.Y, tri.V1.Z), viewport);
            var p2 = camera.Project(new Vector3D(tri.V2.X, tri.V2.Y, tri.V2.Z), viewport);

            if (p0 == ChartPoint.Empty || p1 == ChartPoint.Empty || p2 == ChartPoint.Empty) continue;

            // Simple backface culling in screen space
            var cross = (p1.X - p0.X) * (p2.Y - p0.Y) - (p1.Y - p0.Y) * (p2.X - p0.X);
            if (cross <= 0) continue;

            var shadedColor = _light.ApplyLighting(tri.Color, tri.Normal);
            var pen = new ChartPen(shadedColor.WithAlpha(180), 0.5f);

            // Draw triangle edges to 2D canvas
            canvas.DrawLine(p0, p1, pen);
            canvas.DrawLine(p1, p2, pen);
            canvas.DrawLine(p2, p0, pen);
        }
    }
}
