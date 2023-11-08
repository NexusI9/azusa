using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    //Events
    public event System.Action<Tile> TileHover;

    //Parameters
    public bool locked = false;

    //Private
    private Material material;

    private void Start()
    {
        //Instiantiate new material from existing one
        this.material = new Material(GetComponent<Renderer>().material);

        //Assign new instance to gameObject
        gameObject.GetComponent<Renderer>().material = this.material;
    }

    private void Update()
    {
        //UpdateStrokeWidth();
        HandleLockState();
    }


    private void HandleLockState()
    {

        if (locked)
        {
            this.material.SetColor("_TileColor", Color.red);
        }
        else
        {

        }
    }


    private void OnMouseEnter()
    {
        this.material.SetInt("_Glow", 1);
        //Update LastTilePosition in Grid
        TileHover?.Invoke(this);
    }

    private void OnMouseExit()
    {
        this.material.SetInt("_Glow", 0);
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
        this.material.SetFloat("_StrokeWidth", newStrokeWidth);
    }

}
