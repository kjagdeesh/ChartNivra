namespace ChartNivra.Rendering.Skia
{
    using ChartNivra.Core.Primitives;
    using ChartNivra.Core.ThreeD;
    using System.Numerics;

    public static class Column3DMeshGenerator
    {
        public static List<Triangle3D> CreateCuboid(Vector3 min, Vector3 max, ChartColor color)
        {
            var triangles = new List<Triangle3D>();

            // 8 vertices of cuboid
            var v0 = new Vector3(min.X, min.Y, min.Z);
            var v1 = new Vector3(max.X, min.Y, min.Z);
            var v2 = new Vector3(max.X, max.Y, min.Z);
            var v3 = new Vector3(min.X, max.Y, min.Z);
            var v4 = new Vector3(min.X, min.Y, max.Z);
            var v5 = new Vector3(max.X, min.Y, max.Z);
            var v6 = new Vector3(max.X, max.Y, max.Z);
            var v7 = new Vector3(min.X, max.Y, max.Z);

            // Faces: each face is 2 triangles
            // Bottom (Y = min.Y) - facing down
            AddQuad(triangles, v0, v3, v2, v1, color, new Vector3(0, -1, 0));
            // Top (Y = max.Y) - facing up
            AddQuad(triangles, v4, v5, v6, v7, color, new Vector3(0, 1, 0));
            // Front (Z = min.Z) - facing forward
            AddQuad(triangles, v0, v1, v5, v4, color, new Vector3(0, 0, -1));
            // Back (Z = max.Z) - facing backward
            AddQuad(triangles, v3, v7, v6, v2, color, new Vector3(0, 0, 1));
            // Left (X = min.X) - facing left
            AddQuad(triangles, v0, v4, v7, v3, color, new Vector3(-1, 0, 0));
            // Right (X = max.X) - facing right
            AddQuad(triangles, v1, v2, v6, v5, color, new Vector3(1, 0, 0));

            return triangles;
        }

        private static void AddQuad(List<Triangle3D> triangles, Vector3 a, Vector3 b, Vector3 c, Vector3 d, ChartColor color, Vector3 normal)
        {
            triangles.Add(new Triangle3D(a, b, c, color) { Normal = normal });
            triangles.Add(new Triangle3D(a, c, d, color) { Normal = normal });
        }
    }
}