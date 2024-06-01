using System;
using UnityEngine;
using System.Collections.Generic;
using Utils;

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



    public List<Vector3> mesh = new List<Vector3>();
    private List<Circle> circles = new List<Circle>();

    public Mesh Spawn(Vector2 position)
    {

        Mesh finalMesh = new Mesh();
        List<Circle> circleConfig = new List<Circle>()
        {
            //1. Set Ground
            new Circle(){
                name = "ground",
                segments = segments,
                radius = radius
            },

            //2. Set Belt
            new Circle()
            {
                name= "belt",
                segments = (int) Mathf.Ceil(segments / 2),
                radius = radius / 1.2f,
                position = new Vector3(0, -1 * depth, 0)
            },

            //3. Set Root
            new Circle()
            {
                name = "root",
                segments = (int)Mathf.Ceil(segments / 3),
                radius = radius / 2.5f,
                position = new Vector3(0, -2 * depth, 0),
            }
        };


        //Loop through circles to make them spawn 
        foreach(Circle circle in circleConfig)
        {
            //add circle to global array
            circles.Add(circle);
            circle.Spawn();
        }

        int circlesLength = circles.Count;
        //Bridge circles together
        for (int i = 0; i < circlesLength - 1; i++)
        {
            Circle currentCircle = circles[i];
            Circle nextCircle = circles[(i + 1) % circlesLength];

            BridgeLoop loop = new BridgeLoop(currentCircle.mesh.vertices, nextCircle.mesh.vertices);
            finalMesh = loop.Connect();
        }


        //circles.Add(newCircle);

        return finalMesh;

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
