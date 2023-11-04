using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInteraction : Raycaster
{

    private Material material;
    private MaterialPropertyBlock propertyBlock;
    private Color defaultStrokeColor;
    private Color defaultTileColor;


    private void Start()
    {
        //Instiantiate new material from existing one
        this.material = new Material(GetComponent<Renderer>().material);
        this.defaultStrokeColor= this.material.GetColor("_StrokeColor");
        this.defaultTileColor = this.material.GetColor("_TileColor");
        //Assign new instance to gameObject
        gameObject.GetComponent<Renderer>().material = this.material;
    }

    private void ChangeColor(Color stroke, Color tile)
    {
        this.material.SetColor("_StrokeColor", stroke);
        this.material.SetColor("_TileColor", tile);
    }


    protected override void OnCursorEnter(GameObject hitObject, RaycastHit hit)
    {

        if (GameObject.ReferenceEquals(this.gameObject, hitObject))
        {
            this.ChangeColor(Color.white, Color.white);
        }

    }

    protected override void OnCursorLeave(RaycastHit hit)
    {
        this.ChangeColor(this.defaultStrokeColor, this.defaultTileColor);
    }

}
