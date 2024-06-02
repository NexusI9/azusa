using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class Normal
    {
        public Vector3[] Set(Vector3[] vertices)
        {
            List<Vector3> normals = new List<Vector3>();

            for (int i = 0; i < vertices.Length; i++)
            {
                normals.Add(-Vector3.forward);
            }

            return normals.ToArray();
        }

        public Mesh Flip(Mesh mesh)
        {
            //flip normals
            Vector3[] normals = mesh.normals;
            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = -1 * normals[i];
            }

            mesh.normals = normals;

            //flip triangles order so they face outside
            for (int i = 0; i < mesh.subMeshCount; i++)
            {
                int[] tris = mesh.GetTriangles(i);
    
                for(int j = 0; j < tris.Length; j+=3)
                {
                    //Swap
                    int temp = tris[j];
                    tris[j] = tris[j + 1];
                    tris[j + 1] = temp;
                }

                mesh.SetTriangles(tris, i);
            }


            return mesh;

        }
    }
}

