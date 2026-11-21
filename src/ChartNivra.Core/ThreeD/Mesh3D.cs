namespace ChartNivra.Core.ThreeD;

using ChartNivra.Core.Primitives;
using System.Numerics;

/// <summary>
/// Represents a triangular face in 3D space with vertices and computed surface normal for lighting.
/// </summary>
public class Triangle3D
{
    public Vector3 V0 { get; set; }
    public Vector3 V1 { get; set; }
    public Vector3 V2 { get; set; }
    public ChartColor Color { get; set; }
    public Vector3 Normal { get; set; }

    public Triangle3D(Vector3 v0, Vector3 v1, Vector3 v2, ChartColor color)
    {
        V0 = v0;
        V1 = v1;
        V2 = v2;
        Color = color;
        Normal = Vector3.Normalize(Vector3.Cross(v1 - v0, v2 - v0));
    }

    public float AverageDepth(Matrix4x4 viewMatrix)
    {
        var p0 = Vector4.Transform(new Vector4(V0, 1f), viewMatrix);
        var p1 = Vector4.Transform(new Vector4(V1, 1f), viewMatrix);
        var p2 = Vector4.Transform(new Vector4(V2, 1f), viewMatrix);
        return (p0.Z + p1.Z + p2.Z) / 3f;
    }
}

/// <summary>
/// Simple directional light model for 3D shading.
/// </summary>
public class Light3D
{
    public Vector3 Direction { get; set; } = Vector3.Normalize(new Vector3(-1f, -2f, -1.5f));
    public float AmbientIntensity { get; set; } = 0.35f;
    public float DiffuseIntensity { get; set; } = 0.65f;

    public ChartColor ApplyLighting(ChartColor baseColor, Vector3 normal)
    {
        // Lambertian diffuse factor
        var factor = MathF.Max(0f, Vector3.Dot(normal, -Direction));
        var totalLight = Math.Clamp(AmbientIntensity + factor * DiffuseIntensity, 0f, 1f);

        return new ChartColor(
            (byte)(baseColor.R * totalLight),
            (byte)(baseColor.G * totalLight),
            (byte)(baseColor.B * totalLight),
            baseColor.A
        );
    }
}

/// <summary>
/// 3D Column Mesh generator (creates a 3D rectangular cuboid with 12 triangles).
/// </summary>
public static class Column3DMeshGenerator
{
    public static List<Triangle3D> CreateCuboid(Vector3 min, Vector3 max, ChartColor color)
    {
        var triangles = new List<Triangle3D>(12);

        // 8 Vertices
        var p0 = new Vector3(min.X, min.Y, min.Z);
        var p1 = new Vector3(max.X, min.Y, min.Z);
        var p2 = new Vector3(max.X, max.Y, min.Z);
        var p3 = new Vector3(min.X, max.Y, min.Z);
        var p4 = new Vector3(min.X, min.Y, max.Z);
        var p5 = new Vector3(max.X, min.Y, max.Z);
        var p6 = new Vector3(max.X, max.Y, max.Z);
        var p7 = new Vector3(min.X, max.Y, max.Z);

        // Front Face (Z max)
        triangles.Add(new Triangle3D(p4, p5, p6, color));
        triangles.Add(new Triangle3D(p4, p6, p7, color));

        // Back Face (Z min)
        triangles.Add(new Triangle3D(p1, p0, p3, color));
        triangles.Add(new Triangle3D(p1, p3, p2, color));

        // Top Face (Y max)
        triangles.Add(new Triangle3D(p3, p2, p6, color));
        triangles.Add(new Triangle3D(p3, p6, p7, color));

        // Bottom Face (Y min)
        triangles.Add(new Triangle3D(p0, p1, p5, color));
        triangles.Add(new Triangle3D(p0, p5, p4, color));

        // Right Face (X max)
        triangles.Add(new Triangle3D(p1, p5, p6, color));
        triangles.Add(new Triangle3D(p1, p6, p2, color));

        // Left Face (X min)
        triangles.Add(new Triangle3D(p0, p4, p7, color));
        triangles.Add(new Triangle3D(p0, p7, p3, color));

        return triangles;
    }
}
