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
            Mesh = new Mesh();
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
            return Mesh;
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

            return Mesh;

        }

    }

}
