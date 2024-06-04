using System.Collections.Generic;
using UnityEngine;
using Triangulation;
using Utils;
using System.Linq;

/*
 Circles are the base armature of the islands, they are simply 2d circles deformed
 with noise and that can be triangulated
 */


namespace Island
{

    public interface ICircle
    {
        public int segments { get; set; }
        public float radius { get; set; }
        public float noiseScale { get; set; }
        public float noiseAmplitude { get; set; }
        public float randomness { get; set; }
        public Vector3 position { get; set; }
        public string name { get; set; }
    }


    public class Circle : ICircle
    {

        public int segments { get; set; } = 30;
        public float radius { get; set; } = 1f;
        public float noiseScale { get; set; } = 0.58f;
        public float noiseAmplitude { get; set; } = 13.41f;
        public float randomness { get; set; } = 1.3f;
        public Vector3 position { get; set; } = new Vector3(0, 0, 0);
        public string name { get; set; }
        public bool DebugMode { get; set; } = false;
        public bool Smooth { get; set; } = false;
        public int SmoothThresholdAngle { get; set; } = 110;

        public Mesh mesh { get; private set; }

        public void Spawn()
        {

            Vector2[] points = Points();

            if (Smooth)
            {
                points = SmoothPeak(points, SmoothThresholdAngle);
            }


            Triangulator triangulator = new Triangulator(points);
            mesh = triangulator.mesh;


            Uv uvs = new Uv();
            mesh.uv = uvs.Planar(mesh.vertices);

            Normal normals = new Normal();
            mesh.normals = normals.Set(mesh);


            SetPosition(position);
            //mesh.normals = Normals(mesh.vertices);
            //mesh.uv = Uvs(mesh.vertices);


            if (DebugMode)
            {
                DebugCircle();
            }

        }


        private Vector2[] SmoothPeak(Vector2[] points, float threshold)
        {

            List<Vector2> smoothedPoints = new List<Vector2>();
            int nPeak = 0;


            //prefill list
            for (int i = 0; i < points.Length; i++)
            {
                smoothedPoints.Add(points[i]);
            }

            //Check the angle of each 3 vertices
            //If angle < threshold then add two more vertices between and make peak point closer to ease
            for (int i = 0; i < points.Length; i++)
            {
                Vector2 A = points[i];
                Vector2 B = points[(i + 1) % points.Length];
                Vector2 C = points[(i + 2) % points.Length];

                Vector2 AB = A - B;
                Vector2 BC = C - B;

                bool isPeak = Vector2.Angle(AB, BC) < threshold;

                if (isPeak)
                {

                    Vector2 halfAB = (A + B) / 2;
                    Vector2 halfBC = (C + B) / 2;

                    if (smoothedPoints.Count > i + nPeak + 1)
                    {
                        smoothedPoints[i + nPeak + 1] = (halfAB + B + halfBC) / 3;
                        smoothedPoints.Insert(i + nPeak + 1, halfAB);

                        if (smoothedPoints.Count > i + nPeak + 3)
                        {
                            smoothedPoints.Insert(i + nPeak + 3, halfBC);
                        }
                    }

                    nPeak += 2;
                }

            }

            return smoothedPoints.ToArray();
        }


        private void DebugCircle()
        {

            Debugger.Polygon(new Polygon()
            {
                points = mesh.vertices
            });

            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                Debugger.Label(new Label()
                {
                    text = "" + i,
                    position = mesh.vertices[i] + new Vector3(0, 1, 0)
                });
            }
        }


        private Vector2[] Points()
        {

            //Setup initial points from which the delaunay triangulation will be calculated
            Vector2[] pts = new Vector2[segments];

            float angleStep = 360.0f / segments;
            for (int i = 0; i < segments; i++)
            {

                //generate base circle coordinates
                float angle = Mathf.Deg2Rad * (i * angleStep);
                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;

                //add noise
                float noise = Mathf.PerlinNoise(x * noiseScale, z * noiseScale) * noiseAmplitude;
                float adjustedRadius = radius + noise;
                adjustedRadius += Random.Range(-randomness, randomness);

                pts[i] = new Vector2(x * adjustedRadius, z * adjustedRadius);
            }

            return pts;
        }


        private int[] DefaultTriangles()
        {

            List<int> tris = new List<int>();
            // Triangles
            for (int i = 1; i <= segments; i++)
            {
                int nextIndex = (i % segments) + 1;

                // Triangle made of the center vertex, current vertex, and next vertex
                tris.Add(0); // Center vertex
                tris.Add(nextIndex);
                tris.Add(i);
            }

            return tris.ToArray();
        }

        private Vector3[] DefaultVertices(Vector2[] points)
        {
            List<Vector3> vertices = new List<Vector3>();

            vertices.Add(Vector3.zero);
            // Calculate the vertices around the circle
            for (int p = 0; p < points.Length; p++)
            {
                Vector2 currentPoint = points[p];
                vertices.Add(new Vector3(currentPoint.x, 0, currentPoint.y));
            }

            return vertices.ToArray();
        }

        public void SetPosition(Vector3 position)
        {

            Vector3[] tempVert = mesh.vertices;
            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                tempVert[i] = tempVert[i] + position;
            }

            mesh.vertices = tempVert;
            mesh.RecalculateBounds();
        }
    }


}