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

    //Chunk depth
    public float depth = 3;

    private List<Circle> circles = new List<Circle>();

    public Mesh Spawn(Vector2 position)
    {

        //1. Set Ground
        Circle ground = new Circle()
        {
            segments = segments,
            radius = radius
        };

        //2. Set Belt
        Circle belt = new Circle()
        {
            segments = (int) Mathf.Ceil(segments / 2),
            radius = radius / 1.2f,
            position = new Vector3(0, -1 * depth, 0)
        };

        //3. Set Root

        Circle root = new Circle()
        {
            segments = (int)Mathf.Ceil(segments / 2),
            radius = radius / 2.5f,
            position = new Vector3(0, -2 * depth, 0)
        };


        ground.Spawn();
        belt.Spawn();
        root.Spawn();

        //circles.Add(newCircle);

        return ground.mesh;

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
