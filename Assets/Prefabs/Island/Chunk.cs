using System;
using UnityEngine;
using System.Collections.Generic;


/*
 Chunks are the independant component of an island.
 A chunk basically stacks up many Circles. Chunks turns the 2D basic circles into a SOLO independant island.
 */
public class Chunk
{

    //Base Circle Configuration
    public int segments = 30;
    public float radius = 1f;
    public float noiseScale = 0.58f;
    public float noiseAmplitude = 13.41f;
    public float randomness = 1.3f;

    private List<Circle> circles = new List<Circle>();

    public Mesh Spawn(Vector2 position)
    {
        Circle newCircle = new Circle()
        {
            segments = segments,
            radius = radius,
            noiseScale = noiseScale,
            noiseAmplitude = noiseAmplitude,
            randomness = randomness
        };

        //Build mesh (triangulation)
        Mesh meshCircle = newCircle.Mesh();

        //Update position;
        //SetMeshPosition(meshCircle, position);

        //circles.Add(newCircle);

        return meshCircle;

    }

    private void SetMeshPosition(Mesh mesh, Vector3 position)
    {
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            mesh.vertices[i] = mesh.vertices[i] + position;
        }
    }

    private Mesh MergeCircles()
    {

        Mesh tempMesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();

        foreach (Circle circle in circles)
        {

        }

        return null;
    }


    private void UpdateMesh()
    {
        //gameObject.GetComponent<MeshFilter>().mesh;
    }
}
