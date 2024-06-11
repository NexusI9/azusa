using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;


namespace Utils {


    /*
     *Basic utilities for Mesh manipulation (modifiers)
     */
    public class MeshUtils
    {

        public Mesh Mesh { get; private set; }
        private Mesh BaseMesh;

        public MeshUtils(Mesh mesh)
        {
            BaseMesh = mesh;
            Mesh = mesh;
        }



        public Mesh DownSample(int max)
        {

            if (BaseMesh.vertices.Length > max)
            {
                List<Vector3> vertices = new List<Vector3>();

                for (int i = 0; i < BaseMesh.vertices.Length; i += Mathf.CeilToInt(BaseMesh.vertices.Length / max))
                {
                    vertices.Add(BaseMesh.vertices[i]);
                }

                Mesh.vertices = vertices.ToArray();
            }

            BaseMesh = Mesh;
            return Mesh;
        }

        public Vector2[] ToVector2(Vector3[] vertices)
        {

            List<Vector2> vectors = new List<Vector2>();

            for(int i = 0; i < vertices.Length; i++)
            {
                Vector3 currVert = vertices[i];
                vectors.Add(new Vector2( currVert.x, currVert.z ));
            }

            return vectors.ToArray();

        }

        public Vector3[] ToVector3(Vector2[] vertices, float y = 0.0f)
        {

            List<Vector3> vectors = new List<Vector3>();

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector2 currVert = vertices[i];
                vectors.Add(new Vector3(currVert.x, y, currVert.y));
            }

            return vectors.ToArray();

        }

        public Mesh Shrink(float distance)
        {
            
            List<Vector3> vertices = new List<Vector3>();

            //Get bisect axes and shrink down mesh
            for (int i = 0; i < Mesh.vertices.Length; i++)
            {
                Vector3 A = Mesh.vertices[i == 0 ? Mesh.vertices.Length - 1 : i - 1];
                Vector3 B = Mesh.vertices[i];
                Vector3 C = Mesh.vertices[(i + 1) % Mesh.vertices.Length];

                Vector3 AB = B - A;
                Vector3 BC = C - B;

                Vector3 bisector_norm = (B + AB + BC).normalized;

                Vector3 dir = -1 * distance * (B - bisector_norm).normalized;
                Vector3 shrinkPoint = dir + B;
                vertices.Add(shrinkPoint);
            }


            Mesh.vertices = vertices.ToArray();

            BaseMesh = Mesh;
            return Mesh;

        }


        public Bounds BoundingBox()
        {
            float minX = float.MaxValue, minY = float.MaxValue;
            float maxX = float.MinValue, maxY = float.MinValue;


            foreach (Vector3 vertex in BaseMesh.vertices)
            {
                minX = Mathf.Min(minX, vertex.x);
                minY = Mathf.Min(minY, vertex.y);
                maxX = Mathf.Max(maxX, vertex.x);
                maxY = Mathf.Max(maxY, vertex.y);
            }

            return new Bounds(
                new Vector2((minX + maxX) / 2, (minY + maxY) / 2),
                new Vector2(maxX - minX, maxY - minY)
            );

        }

    }

}
