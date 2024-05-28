using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Island : MonoBehaviour
{
    public int segments = 30;
    public float radius = 1f;
    public float noiseScale = 0.2f;
    public float noiseAmplitude = 10f;
    public float randomness = 12f;

    void Start()
    {
        Circle topCircle = new Circle() {
            segments = segments,
            radius = radius,
            noiseScale = noiseScale,
            noiseAmplitude = noiseAmplitude,
            randomness = randomness
        };
        Mesh topCircleMesh = topCircle.Mesh();

        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<MeshFilter>().mesh = topCircleMesh;
        //gameObject.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
    }

    private void OnDrawGizmos()
    {
        DrawVertices();
    }

    private void DrawVertices()
    {
        if (!gameObject.GetComponent<MeshFilter>()) { return;  } 
        Vector3[] vert = gameObject.GetComponent<MeshFilter>().mesh.vertices;

        for(int i = 0; i < vert.Length; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(vert[i], 0.3f);
            Gizmos.DrawLine(vert[i], vert[(i+1)%vert.Length]);
        }
    }

    


}

