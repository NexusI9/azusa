using System;
using System.Collections.Generic;
using UnityEngine;


namespace Utils
{


    public static class Uv
    {

        public static Vector2[] Planar(Vector3[] vertices)
        {

            //Planar projection
            Vector2[] uvs = new Vector2[vertices.Length];
            float minX = float.MaxValue, maxX = float.MinValue, minY = float.MaxValue, maxY = float.MinValue;

            //get min max values
            foreach (Vector3 vertex in vertices)
            {
                minX = Mathf.Min(minX, vertex.x);
                minY = Mathf.Min(minY, vertex.z);

                maxX = Mathf.Max(maxX, vertex.x);
                maxY = Mathf.Max(maxY, vertex.z);
            }

            float width = maxX - minX;
            float height = maxY - minY;

            for (int i = 0; i < vertices.Length; i++)
            {
                float u = (vertices[i].x - minX) / width;
                float v = (vertices[i].z - minY) / height;
                uvs[i] = new Vector2(u, v);
            }

            return uvs;
        }

        public static Vector2[] Cylindrical(Vector3[] vertices)
        {

            Vector2[] uvs = new Vector2[vertices.Length];
            //Define bounding box around Y axis

            float minY = float.MaxValue, maxY = float.MinValue;

            for(int i = 0; i < vertices.Length; i++)
            {
                minY = Mathf.Min(minY, vertices[i].y);
                maxY = Mathf.Max(maxY, vertices[i].y);
            }

            float height = maxY - minY;

            for(int i =0; i < vertices.Length; i++)
            {
                // Calculate the angle around the Y-axis
                float angle = Mathf.Atan2(vertices[i].z, vertices[i].x);
                // Normalize angle to [0, 1]
                float u = (angle + Mathf.PI) / (2 * Mathf.PI);

                // Normalize Y coordinate to [0, 1]
                float v = (vertices[i].y - minY) / height;

                uvs[i] = new Vector2(u, v);
            }

            return uvs; 
        }


    }


}