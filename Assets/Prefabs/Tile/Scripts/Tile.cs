using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : GridMaster
{

    public bool locked = false;

    private Material material;


    private void Start()
    {
        //Instiantiate new material from existing one
        this.material = new Material(GetComponent<Renderer>().material);

        //Assign new instance to gameObject
        gameObject.GetComponent<Renderer>().material = this.material;
    }


    public void SetLastTile(Vector3 tilePosition)
    {
        lastTilePosition = tilePosition;
    }


    private void OnMouseEnter()
    {
        this.material.SetInt("_Glow", 1);
        //Update LastTilePosition in Grid
        gameObject.GetComponent<Tile>().SetLastTile(gameObject.transform.position);

    }

    private void OnMouseExit()
    {
        this.material.SetInt("_Glow", 0);
    }

}
