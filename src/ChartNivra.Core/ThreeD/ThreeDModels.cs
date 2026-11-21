namespace ChartNivra.Core.ThreeD;

using ChartNivra.Core.Primitives;
using System.Numerics;

/// <summary>
/// Represents a point in 3D world space (X, Y, Z).
/// </summary>
public readonly record struct Vector3D(double X, double Y, double Z)
{
    public static readonly Vector3D Zero = new(0, 0, 0);
}

/// <summary>
/// Perspective camera model with orbit, pan, and zoom capabilities.
/// </summary>
public class Camera3D
{
    public Vector3 Position { get; set; } = new(10f, 10f, 15f);
    public Vector3 Target { get; set; } = Vector3.Zero;
    public Vector3 Up { get; set; } = Vector3.UnitY;
    public float FieldOfViewDegrees { get; set; } = 45f;
    public float NearPlane { get; set; } = 0.1f;
    public float FarPlane { get; set; } = 1000f;

    public Matrix4x4 GetViewMatrix() =>
        Matrix4x4.CreateLookAt(Position, Target, Up);

    public Matrix4x4 GetProjectionMatrix(float aspectRatio) =>
        Matrix4x4.CreatePerspectiveFieldOfView(
            FieldOfViewDegrees * (MathF.PI / 180f),
            aspectRatio,
            NearPlane,
            FarPlane
        );

    /// <summary>
    /// Projects a 3D world coordinate to 2D screen coordinate pixels.
    /// </summary>
    public ChartPoint Project(Vector3D point, ChartRect viewport)
    {
        var v = new Vector3((float)point.X, (float)point.Y, (float)point.Z);
        var view = GetViewMatrix();
        var proj = GetProjectionMatrix((float)(viewport.Width / Math.Max(1.0, viewport.Height)));

        var clipSpace = Vector4.Transform(new Vector4(v, 1.0f), view * proj);
        if (clipSpace.W == 0) return ChartPoint.Empty;

        // Normalized Device Coordinates (NDC) [-1, 1]
        var ndcX = clipSpace.X / clipSpace.W;
        var ndcY = clipSpace.Y / clipSpace.W;

        var screenX = viewport.Left + (ndcX + 1.0) * 0.5 * viewport.Width;
        var screenY = viewport.Top + (1.0 - ndcY) * 0.5 * viewport.Height; // Invert Y

        return new ChartPoint(screenX, screenY);
    }
}

/// <summary>
/// 3D data point representation.
/// </summary>
public class Point3D
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
    public ChartColor Color { get; set; } = ChartColor.Transparent;

    public Point3D(double x, double y, double z)
    {
        X = x; Y = y; Z = z;
    }
}
