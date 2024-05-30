using System.Collections.Generic;
using UnityEngine;
using Triangulation;
using Utils;

/*
 Circles are the base armature of the islands, they are simply 2d circles deformed
 with noise and that can be triangulated
 */
public class Circle
{

    public int segments { get; set; }
    public float radius { get; set; }
    public float noiseScale { get; set; }
    public float noiseAmplitude { get; set; }
    public float randomness { get; set; }

    private Mesh mesh = new Mesh();

    public Mesh Mesh()
    {

        Vector2[] points = Points();
        Triangulator triangulator = new Triangulator(points);
        mesh = triangulator.mesh;

        //mesh.vertices = DefaultVertices(points);
        //mesh.triangles = DefaultTriangles();
        //mesh.normals = Normals(mesh.vertices);
        //mesh.uv = Uvs(mesh.vertices);

        Debugger.DrawPolygon(new Polygon()
        {
            points = mesh.vertices
        }) ;

        return mesh;
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


    private Vector2[] Uvs(Vector3[] vertices)
    {
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x / (radius * 2) + 0.5f, vertices[i].y / (radius * 2) + 0.5f);
        }

        return uvs;
    }

    private Vector3[] Normals(Vector3[] vertices)
    {
        List<Vector3> normals = new List<Vector3>();

        for (int i = 0; i < vertices.Length; i++)
        {
            normals.Add(-Vector3.forward);
        }

        return normals.ToArray();
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
}
