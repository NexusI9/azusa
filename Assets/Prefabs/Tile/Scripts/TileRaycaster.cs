using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInteraction : Raycaster
{

    private Material material;


    private void Start()
    {
        //Instiantiate new material from existing one
        this.material = new Material(GetComponent<Renderer>().material);

        //Assign new instance to gameObject
        gameObject.GetComponent<Renderer>().material = this.material;
    }


    protected override void OnCursorEnter(GameObject hitObject)
    {

        if (this.IsTargeted(gameObject, hitObject))
        {
            this.material.SetInt("_Glow", 1);
            //Update LastTilePosition in Grid
            gameObject.GetComponent<Tile>().SetLastTile(gameObject.transform.position); 
        }
        else
        {
            this.material.SetInt("_Glow", 0);
        }

    }

    protected override void OnCursorLeave(GameObject hitObject)
    {
        if (this.IsTargeted(gameObject, hitObject))
        {
            this.material.SetInt("_Glow", 0);
        }
            
    }

}
