using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class Normal
    {
        public Vector3[] Set(Mesh mesh)
        {

            Vector3[] vertices = mesh.vertices;
            int[] triangles = mesh.triangles;
            Vector3[] normals = new Vector3[vertices.Length];

            // Initialize all normals to zero
            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = Vector3.zero;
            }

            // Calculate normals for each triangle and add to the vertices
            for (int i = 0; i < triangles.Length; i += 3)
            {
                int index0 = triangles[i];
                int index1 = triangles[i + 1];
                int index2 = triangles[i + 2];

                Vector3 v0 = vertices[index0];
                Vector3 v1 = vertices[index1];
                Vector3 v2 = vertices[index2];

                Vector3 normal = Vector3.Cross(v1 - v0, v2 - v0).normalized;

                normals[index0] += normal;
                normals[index1] += normal;
                normals[index2] += normal;
            }

            // Normalize all vertex normals
            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = normals[i].normalized;
            }

            return mesh.normals;
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

        public Vector3[] Smooth(Mesh mesh)
        {


            return mesh.normals;
        }

        
    }
}

