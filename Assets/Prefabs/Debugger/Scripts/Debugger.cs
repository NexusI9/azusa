using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;




public class Debugger : MonoBehaviour
{

    private static Debugger _instance;
    public static Debugger Instance
    {
        get
        {
            return _instance;
        }
    }

    private static List<Vector3[]> polygons = new List<Vector3[]>();

    private void Awake()
    {
        _instance = this;
    }

    public static void DrawPolygon(Vector3[] points)
    {
        polygons.Add(points);
    }


    private void DrawVertices(Vector3[] points)
    {
        for (int i = 0; i < points.Length; i++)
        {
            Gizmos.DrawWireCube(points[i], new Vector3(0.3f, 0.3f, 0.3f));
        }
    }

    private void DrawEdges(Vector3[] points)
    {
        for (int i = 0; i < points.Length; i++)
        {
            Gizmos.DrawLine(points[i], points[(i + 1) % points.Length]);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        foreach(Vector3[] polygon in polygons)
        {
            DrawVertices(polygon);
            DrawEdges(polygon);
        }

    }
}
