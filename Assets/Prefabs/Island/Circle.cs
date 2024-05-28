using System.Collections.Generic;
using UnityEngine;
using DelaunatorSharp;

public class Circle : MonoBehaviour
{

    public int segments { get; set; }
    public float radius { get; set; }
    public float noiseScale { get; set; }
    public float noiseAmplitude { get; set; }
    public float randomness { get; set; }

    private Mesh mesh = new Mesh();

    public Circle() { }

    public Mesh Mesh()
    {

        IPoint[] points = Points();
        Delaunator dn = new Delaunator(points);

        mesh.name = "Circle";
        mesh.vertices = Vertices(dn);
        mesh.triangles = Triangles(dn);

        //mesh.vertices = DefaultVertices(points);
        //mesh.triangles = DefaultTriangles();

        //mesh.normals = Normals(mesh.vertices);
        //mesh.uv = Uvs(mesh.vertices);

        return mesh;
    }

    private IPoint[] Points()
    {

        //Setup initial points from which the delaunay triangulation will be calculated
        IPoint[] pts = new IPoint[segments];

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

            pts[i] = new Point(x * adjustedRadius, z * adjustedRadius);
        }

        return pts;
    }

    private Vector3[] Vertices(Delaunator dn)
    {

        List<Vector3> vertices = new List<Vector3>();
 
        foreach (IPoint pt in dn.Points)
        {
            //Get points stored in triangles
            vertices.Add(new Vector3((float)pt.X, 0, (float)pt.Y));
        }

        return vertices.ToArray();
    }



    private int[] Triangles(Delaunator dn)
    {
        IEnumerable<ITriangle> triangles = dn.GetTriangles();
        List<int> tris = new List<int>();
        //int tIndex = 0;

        for(int i = 0; i < dn.Triangles.Length; i++)
        {
            Debug.Log(dn.Triangles);
            tris.Add(dn.Triangles[i]);
        }

        /*foreach (ITriangle t in triangles)
        {
            foreach (int index in dn.PointsOfTriangle(tIndex))
            {
                Debug.Log(index);
                //tris.Add(index);
            }
            tIndex++;
        }*/

        return tris.ToArray();
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

    private Vector3[] DefaultVertices(IPoint[] points)
    {
        List<Vector3> vertices = new List<Vector3>();

        vertices.Add(Vector3.zero);
        // Calculate the vertices around the circle
        for (int p = 0; p < points.Length; p++)
        {
            IPoint currentPoint = points[p];
            vertices.Add(new Vector3((float)currentPoint.X, 0, (float)currentPoint.Y));
        }

        return vertices.ToArray();
    }
}
