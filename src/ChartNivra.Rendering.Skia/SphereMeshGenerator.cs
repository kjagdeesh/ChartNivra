namespace ChartNivra.Rendering.Skia
{
    using ChartNivra.Core.Primitives;
    using ChartNivra.Core.ThreeD;
    using System.Numerics;

    public static class SphereMeshGenerator
    {
        public static List<Triangle3D> CreateSphere(Vector3 center, float radius, int segments, ChartColor color)
        {
            var triangles = new List<Triangle3D>();

            for (int lat = 0; lat < segments; lat++)
            {
                var theta1 = lat * Math.PI / segments;
                var theta2 = (lat + 1) * Math.PI / segments;

                var sin1 = (float)Math.Sin(theta1);
                var cos1 = (float)Math.Cos(theta1);
                var sin2 = (float)Math.Sin(theta2);
                var cos2 = (float)Math.Cos(theta2);

                for (int lon = 0; lon < segments; lon++)
                {
                    var phi1 = lon * 2 * Math.PI / segments;
                    var phi2 = (lon + 1) * 2 * Math.PI / segments;

                    var sinPhi1 = (float)Math.Sin(phi1);
                    var cosPhi1 = (float)Math.Cos(phi1);
                    var sinPhi2 = (float)Math.Sin(phi2);
                    var cosPhi2 = (float)Math.Cos(phi2);

                    var v0 = new Vector3(
                        center.X + radius * sin1 * cosPhi1,
                        center.Y + radius * cos1,
                        center.Z + radius * sin1 * sinPhi1);

                    var v1 = new Vector3(
                        center.X + radius * sin2 * cosPhi1,
                        center.Y + radius * cos2,
                        center.Z + radius * sin2 * sinPhi1);

                    var v2 = new Vector3(
                        center.X + radius * sin2 * cosPhi2,
                        center.Y + radius * cos2,
                        center.Z + radius * sin2 * sinPhi2);

                    var v3 = new Vector3(
                        center.X + radius * sin1 * cosPhi2,
                        center.Y + radius * cos1,
                        center.Z + radius * sin1 * sinPhi2);

                    var n0 = new Vector3(v0.X - center.X, v0.Y - center.Y, v0.Z - center.Z);
                    var n1 = new Vector3(v1.X - center.X, v1.Y - center.Y, v1.Z - center.Z);
                    var n2 = new Vector3(v2.X - center.X, v2.Y - center.Y, v2.Z - center.Z);
                    var n3 = new Vector3(v3.X - center.X, v3.Y - center.Y, v3.Z - center.Z);

                    triangles.Add(new Triangle3D(v0, v1, v2, color) { Normal = n0 });
                    triangles.Add(new Triangle3D(v0, v2, v3, color) { Normal = n0 });
                }
            }

            return triangles;
        }
    }
}