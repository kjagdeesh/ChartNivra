namespace ChartNivra.Rendering.ThreeD.Abstractions;

using ChartNivra.Core.Primitives;
using ChartNivra.Core.ThreeD;

/// <summary>
/// Depth sorting strategy for 3D triangle rasterization.
/// </summary>
public enum DepthSortStrategy
{
    PaintersAlgorithmCentroid,
    PaintersAlgorithmNearestVertex,
    PaintersAlgorithmFarthestVertex
}

/// <summary>
/// Geometry buffer contract for holding 3D vertices, normals, and triangle indices.
/// </summary>
public interface IGeometryBuffer3D
{
    IReadOnlyList<Vector3D> Vertices { get; }
    IReadOnlyList<Vector3D> Normals { get; }
    IReadOnlyList<int> Indices { get; }
    void AddVertex(Vector3D vertex, Vector3D normal);
    void AddTriangle(int i0, int i1, int i2);
    void Clear();
}

/// <summary>
/// Render pipeline for 3D scenes.
/// </summary>
public interface IRenderPipeline3D
{
    void RenderScene(IGeometryBuffer3D geometry, Camera3D camera, ChartRect viewport);
}
