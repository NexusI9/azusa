using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : GridMaster
{

    public bool locked = false;
    public bool active = false;
    public GameObject child;


    // Start is called before the first frame update
    private void Start()
    {
        if (!child) { return; }

        child = this.InstanceChild(child);
        this.AddChildScript(child);

    }

    // Update is called once per frame
    private void Update()
    {
        if (active == true)
        {

            MoveTo(lastTilePosition);
        }


    }

    private GameObject InstanceChild(GameObject child)
    {

        //Add child to the parent GridObject
        // Instantiate a copy of the child prefab as a new GameObject
        GameObject newChild = Instantiate(child);

        // Set the parent of the new child GameObject to the parentObject
        newChild.transform.parent = gameObject.transform;

        // Optionally, you can adjust the position and rotation of the new child
        newChild.transform.localPosition = Vector3.zero;
        newChild.transform.localRotation = Quaternion.identity;
        return newChild;
    }

    private void AddChildScript(GameObject child)
    {
        child.AddComponent<GridObjectRaycaster>(); 
    }

    private void MoveTo(Vector3 newPosition)
    {
        Debug.Log(lastTilePosition);
        if (newPosition == null) { return; }
        // Move the GameObject to the rounded grid position with a specified speed
        transform.position = newPosition; 
    }
}
