using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class Label
{
    public string text { get; set; }
    public Vector3 position { get; set; }
}

public class Polygon
{
    public Vector3[] points { get; set; }
    public bool label { get; set; } = false;
    public bool edges { get; set; } = true;
    public Color color { get; set; } = Color.green;
}



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

    private static List<Polygon> polygons = new List<Polygon>();
    private static List<Label> labels = new List<Label>();

    private void Awake()
    {
        _instance = this;
    }

    public static void DrawPolygon(Polygon points)
    {
        polygons.Add(points);
    }

    public static void DrawLabel(Label label)
    {
        labels.Add(label);
    }


    private void DrawLabelGizmos(Label label)
    {
        #if UNITY_EDITOR
                UnityEditor.Handles.color = Color.green;
                UnityEditor.Handles.Label(label.position, label.text);
        #endif
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

        //Draw Polygons
        foreach(Polygon polygon in polygons)
        {
            Gizmos.color = polygon.color;

            //Display vertices
            DrawVertices(polygon.points);

            //Display edges
            if (polygon.edges) DrawEdges(polygon.points);

            //Display points locations for each points
            if (polygon.label)
            {
                for(int p = 0; p < polygon.points.Length; p++)
                {
                    Vector3 currentPoint = polygon.points[p];
                    DrawLabel(new Label()
                    {
                        text = "x:" + currentPoint.x + ", y:" + currentPoint.y + ", z:" + currentPoint.z,
                        position = currentPoint + new Vector3(0.5f, 1, 0)
                    });
                }
            }
        }

        //Draw Labels
        foreach (Label label in labels)
        {
            DrawLabelGizmos(label);
        }
    }

    private void OnDisable()
    {
        polygons.Clear();
        labels.Clear();
    }
}
