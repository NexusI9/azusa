using UnityEngine;
using System.Collections.Generic;

/*
 Islands are a gathering of chunks
 */

public class Island : MonoBehaviour
{

    private List<Chunk> Chunks = new List<Chunk>();
    public Material GroundMaterial;
    public Material RockMaterial;

    private void Start()
     {

         Chunk baseChunk = new Chunk();
         gameObject.AddComponent<MeshFilter>();
         gameObject.AddComponent<MeshRenderer>();
         gameObject.GetComponent<MeshFilter>().mesh = baseChunk.Spawn(new Vector2(0, 0));
         
         gameObject.GetComponent<MeshRenderer>().materials = new Material[] { GroundMaterial, RockMaterial};

     }

}


