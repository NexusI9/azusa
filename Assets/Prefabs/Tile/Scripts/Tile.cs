using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : GridMaster
{

    //Events
    public event System.Action<Tile> TileHover;

    //Parameters
    public bool locked = false;

    //Private
    private Material material;
    private Color defaultTileColor;

    private void Start()
    {
        //Instiantiate new material from existing one
        material = new Material(GetComponent<Renderer>().material);

        //Assign new instance to gameObject
        gameObject.GetComponent<Renderer>().material = material;
        defaultTileColor = material.GetColor("_TileColor");
    }

    private void Update()
    {
        //UpdateStrokeWidth();
    }


    public void HandleLockState(bool locked)
    {
        if (locked)
        {
            material.SetColor("_TileColor", Color.red);
        }
        else
        {
            material.SetColor("_TileColor", defaultTileColor);
        }
    }

    public void SetGlow(bool glow)
    {
        if (glow)
        {
            material.SetInt("_Glow", 1);
        }
        else
        {
            material.SetInt("_Glow", 0);
        }
    }


    private void OnMouseEnter()
    {

        SetGlow(true);
        //Update LastTilePosition in Grid
        lastActiveTile = this;
        TileHover?.Invoke(this);
    }

    private void OnMouseExit()
    {
        SetGlow(false);
    }

    private void UpdateStrokeWidth()
    {

        float maxDistance = 10.0f;
        float maxStrokeWidth = 1.0f; // Set your desired maximum stroke width

        // Calculate the distance to the camera
        float distanceToCamera = Vector3.Distance(transform.parent.position, Camera.main.transform.position);

        // Calculate the new stroke width value based on the distance
        float newStrokeWidth = maxStrokeWidth - (maxStrokeWidth / maxDistance) * distanceToCamera;

        // Make sure the stroke width doesn't go below 0
        newStrokeWidth = Mathf.Max(newStrokeWidth, 0f) * 20;

        Debug.Log(newStrokeWidth);
        // Update the shader property in the material
        material.SetFloat("_StrokeWidth", newStrokeWidth);
    }

}
