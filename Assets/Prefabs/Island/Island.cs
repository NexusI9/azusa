using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Island : MonoBehaviour
{
    private int segments = 100;
    private float radius = 5f;

    // Start is called before the first frame update
    void Start()
    {
        Circle topCircle = new Circle() { segments = segments, radius = radius };
        Mesh topCircleMesh = topCircle.Mesh();

        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
         
        gameObject.GetComponent<MeshFilter>().mesh = topCircleMesh;
        //gameObject.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));

        //gameObject.transform.localScale = new Vector3(10, 10, 10);

 

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
        }
    }

    


}

