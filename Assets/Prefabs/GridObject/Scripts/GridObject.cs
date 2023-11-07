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

    }

    // Update is called once per frame
    private void Update()
    {
        HandleRaycaster();
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


    private void HandleMovement(Vector3 newPosition)
    {
        if (newPosition == null || active == false) { return; }
        // Move the GameObject to the rounded grid position with a specified speed
        transform.position = newPosition; 
    }

    private void OnMouseDown()
    {
        Debug.Log("mouseDown");
        //activate selected object
        this.active = true;
    }

    private void OnMouseUp()
    {
        //deactivate selected objet
        this.active = false;
    }

    private void HandleRaycaster()
    {

        GameObject hitObject = Raycaster.GetHitObject();
        if (hitObject != null)
        {
            if (hitObject.transform.IsChildOf(transform))
            {
                // The click happened on a child GameObject
                HandleChildClick(hitObject);
            }
            else
            {
                // The click happened on the parent GameObject
                HandleParentClick();
            }
        }
    }

    private void HandleChildClick(GameObject child)
    {

    }

    private void HandleParentClick()
    {
        HandleMovement(lastTilePosition);
    }

    private void AdjustColliderSize()
    {
        // Get all child colliders
        Collider[] childColliders = GetComponentsInChildren<Collider>();

        // Initialize parent collider size
        Bounds parentBounds = new Bounds(transform.position, Vector3.zero);

        // Calculate bounds of all child colliders
        foreach (Collider childCollider in childColliders)
        {
            parentBounds.Encapsulate(childCollider.bounds);
        }

        // Update the parent collider size
        BoxCollider parentCollider = GetComponent<BoxCollider>();
        parentCollider.center = parentBounds.center - transform.position;
        parentCollider.size = parentBounds.size;
    }

}
