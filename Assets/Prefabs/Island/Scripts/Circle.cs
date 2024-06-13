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

        public Vector3[] OuterVertices { get; private set; } = new Vector3[] { };
        public Vector3[][] InnerVertices { get; private set; } = new Vector3[][] { };

        public float[] InnerCircles { get; set; } = new float[] { };

        public Mesh mesh { get; private set; }

        public void Spawn()
        {

            //Generate Outer Ring
            Vector2[] points = Points();
            if (Smooth) points = SmoothPeak(points, SmoothThresholdAngle);

            OuterVertices = MeshUtils.ToVector3(points);

            //Generate Inner Ring
            if (InnerCircles.Length > 0)
            {
                List<Vector3[]> inners = new List<Vector3[]>();
                for (int i = 0; i < InnerCircles.Length; i++)
                {
                    Vector2[] shrinkCircle = GenerateShrinkCircles(points, InnerCircles[i]);
                    //Adding Inner ring to global array
                    inners.Add(MeshUtils.ToVector3(shrinkCircle));
                }
                InnerVertices = inners.ToArray();
            }

            mesh = new Mesh();
            mesh = CombineRings();

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

        private Mesh CombineRings()
        {

            if(InnerVertices.Length > 0)
            {

                //Generate triangulation from concating outer (n) + inner (n+1) vertices
                List<Vector3[]> outerInner = new List<Vector3[]>();
                outerInner.Add(OuterVertices);
                for(int i = 0; i < InnerVertices.Length; i++)
                {
                   outerInner.Add(InnerVertices[i]);
                }

                CombineInstance[] combine = new CombineInstance[outerInner.Count];

                for (int i = 0; i < outerInner.Count; i++)
                {

                    Vector3[] currentRing = outerInner[i];
                    List<Vector3> concat = new List<Vector3>();
                    concat = concat.Concat(currentRing).ToList();

                    if (i < outerInner.Count-1)
                    {
                        Vector3[] nextRing = outerInner[i + 1];
                        //Concat vertices
                        concat = concat.Concat(nextRing).ToList();

                    }


                    Triangulator triangulator = new Triangulator(MeshUtils.ToVector2(concat.ToArray()));
                    combine[i].mesh = triangulator.mesh;

                }

                Mesh combinedMesh = new Mesh();
                combinedMesh.CombineMeshes(combine, true, false);

                return combinedMesh;
            }
            else
            {
                //Generate first triangulation
                Triangulator triangulator = new Triangulator(MeshUtils.ToVector2(OuterVertices));
                return triangulator.mesh;
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
                Points = mesh.vertices
            });

            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                Debugger.Label(new Label()
                {
                    Text = "" + i,
                    Position = mesh.vertices[i] + new Vector3(0, 1, 0)
                });
            }
        }


        private Vector2[] Points()
        {

            //Setup initial points from which the delaunay triangulation will be calculated
            List<Vector2> pts = new List<Vector2>();

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

                pts.Add(new Vector2(x * adjustedRadius, z * adjustedRadius));
            }


            return pts.ToArray();
        }

        private Vector2[] GenerateShrinkCircles (Vector2[] pts, float distance)
        {

            List<Vector2> listPts = pts.ToList();
            Mesh tempMesh = new Mesh();
            Vector3[] vertices = MeshUtils.ToVector3(pts);

            tempMesh.vertices = vertices;

            Mesh shrinkMesh = MeshUtils.Shrink(tempMesh, distance);

            return MeshUtils.ToVector2(shrinkMesh.vertices);

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

            //Update Mesh Vertices
            Vector3[] tempVert = mesh.vertices;
            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                tempVert[i] += position;
            }

            mesh.vertices = tempVert;
            mesh.RecalculateBounds();

            //Update OuterVertices
            for (int i = 0; i < OuterVertices.Length; i++)
            {
                OuterVertices[i] += position;
            }

            //Update InnerVertices
            for (int i = 0; i < InnerVertices.Length; i++)
            {
                for(int v = 0; v < InnerVertices[i].Length; v++)
                {
                    InnerVertices[i][v] += position;
                }
            }

        }
    }


}