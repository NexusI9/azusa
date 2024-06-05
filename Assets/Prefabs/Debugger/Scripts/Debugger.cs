using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Utils
{

    public class Label
    {
        public string Text { get; set; }
        public Vector3 Position { get; set; }
    }

    public class Polygon
    {
        public Vector3[] Points { get; set; }
        public bool Label { get; set; } = false;
        public bool Edges { get; set; } = true;
        public Color Color { get; set; } = Color.green;
    }

    public class Cube
    {
        public Vector3 Position { get; set; }
        public Vector3 Size { get; set; }
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

        private static List<Polygon> Polygons = new List<Polygon>();
        private static List<Label> Labels = new List<Label>();
        private static List<Cube> Cubes = new List<Cube>();

        private void Awake()
        {
            _instance = this;
        }

        public static void Polygon(Polygon points)
        {
            Polygons.Add(points);
        }

        public static void Label(Label label)
        {
            Labels.Add(label);
        }

        public static void Cube(Cube cube)
        {
            Cubes.Add(cube);
        }

        public static void Log(object obj)
        {
            var output = JsonUtility.ToJson(obj, true);
            Debug.Log(output);
        }


        private void DrawLabelGizmos(Label label)
        {
#if UNITY_EDITOR
            UnityEditor.Handles.color = Color.green;
            UnityEditor.Handles.Label(label.Position, label.Text);
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
            foreach (Polygon polygon in Polygons)
            {
                Gizmos.color = polygon.Color;

                //Display vertices
                DrawVertices(polygon.Points);

                //Display edges
                if (polygon.Edges) DrawEdges(polygon.Points);

                //Display points locations for each points
                if (polygon.Label)
                {
                    for (int p = 0; p < polygon.Points.Length; p++)
                    {
                        Vector3 currentPoint = polygon.Points[p];
                        Label(new Label()
                        {
                            Text = "x:" + currentPoint.x + ", y:" + currentPoint.y + ", z:" + currentPoint.z,
                            Position = currentPoint + new Vector3(0.5f, 1, 0)
                        });
                    }
                }
            }

            //Draw Labels
            foreach (Label label in Labels)
            {
                DrawLabelGizmos(label);
            }

            foreach(Cube cube in Cubes)
            {
               Gizmos.DrawWireCube(cube.Position, cube.Size);
            }


        }

        private void OnDisable()
        {
            Polygons.Clear();
            Labels.Clear();
        }
    }

}
