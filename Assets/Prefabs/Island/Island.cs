using UnityEngine;
using System.Collections.Generic;

/*
 Islands are a gathering of chunks
 */

public class Island : MonoBehaviour
{

    private List<Chunk> chunks = new List<Chunk>();

    private void Start()
     {

         Chunk baseChunk = new Chunk();
         gameObject.AddComponent<MeshFilter>();
         gameObject.AddComponent<MeshRenderer>();
         gameObject.GetComponent<MeshFilter>().mesh = baseChunk.Spawn(new Vector2(0, 0));
         //gameObject.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));

     }

}


