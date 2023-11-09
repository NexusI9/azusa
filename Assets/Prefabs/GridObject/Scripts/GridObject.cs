using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{

    //Events
    public event System.Action<GridObject> MouseUp;
    public event System.Action<GridObject> OnSelected;

    //Parameters
    public GameObject child;
    public int stackable = 0;
    public int _zLength = 1;
    public int _xLength = 1;

    [HideInInspector]
    public bool locked = false;
    [HideInInspector]
    public bool active = false;


    private void Start()
    {
        if (!child) { return; }

        child = this.InstanceChild(child);

    }


    private void Update()
    {
        if (active)
        {

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
        //newChild.transform.localPosition = Vector3.zero;
        newChild.transform.localRotation = Quaternion.identity;
        return newChild;
    }


    public void MoveTo(Vector3 newPosition)
    {
        // Move the GameObject to the rounded grid position with a specified speed
        transform.position = newPosition; 
    }

    private void OnMouseDown()
    {
        //activate selected object
        this.active = true;
        OnSelected.Invoke(this);
    }

    private void OnMouseUp()
    {
        //deactivate selected objet
        this.active = false;
        MouseUp?.Invoke(this);
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
        }
    }

    private void HandleChildClick(GameObject child)
    {
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
