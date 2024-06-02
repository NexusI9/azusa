using System.Collections.Generic;
using UnityEngine;
using Triangulation;
using Utils;

/*
 Circles are the base armature of the islands, they are simply 2d circles deformed
 with noise and that can be triangulated
 */

public interface ICircle
{
    public int segments { get; set; }
    public float radius { get; set; }
    public float noiseScale { get; set; }
    public float noiseAmplitude { get; set; } 
    public float randomness { get; set; }
    public Vector3 position { get; set; }
    public string name { get; set;  }
}


public class Circle : ICircle
{

    public int segments { get; set; } = 30;
    public float radius { get; set; } = 1f;
    public float noiseScale { get; set; } = 0.58f;
    public float noiseAmplitude { get; set; } = 13.41f;
    public float randomness { get; set; } = 1.3f;
    public Vector3 position { get; set; } = new Vector3(0,0,0);
    public string name { get; set; }
    public bool debug { get; set; } = false;

    public Mesh mesh { get; private set; }

    public void Spawn()
    {

        Vector2[] points = Points();


        Triangulator triangulator = new Triangulator(points);
        mesh = triangulator.mesh;

        Uv uvs = new Uv();
        mesh.uv = uvs.Planar(mesh.vertices);

        Normal normals = new Normal();
        mesh.normals = normals.Set(mesh);

        SetPosition(position);
        //mesh.normals = Normals(mesh.vertices);
        //mesh.uv = Uvs(mesh.vertices);


        if (debug)
        {
            DebugCircle();
        }

    }


    private void DebugCircle() {

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
        for(int i = 0; i < mesh.vertices.Length; i++)
        {
            tempVert[i] = tempVert[i] + position;
        }

        mesh.vertices = tempVert;
        mesh.RecalculateBounds();
    }
}
