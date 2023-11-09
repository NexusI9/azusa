using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum StackType 
{
    ADDITIVE, //simply put on top
    EVOLUTIVE //switch instance on stack
}

public class GridObject : MonoBehaviour
{

    //Events
    public event System.Action<GridObject> MouseUp;
    public event System.Action<GridObject> OnSelected;
    public event System.Action<GridObject> MouseEnter;

    //Parameters
    public int ID = 0;

    [Space]
    public GameObject child;
    public int _zSize = 1;
    public int _ySize = 1;
    public int _xSize = 1;

    [Space]
    [SerializeField]
    StackType _stackType = new StackType();
    public int _maxStack = 0;


    [HideInInspector]
    public bool locked = false;
    [HideInInspector]
    public bool active = false;

    //Private
    private List<GridObject> stackArray;
    private GridObject stackBase;

    private void Start()
    {
        if (!child) { return; }

        child = this.InstanceChild(child);
        SetColliderSize();
        stackArray = new List<GridObject>();
    }

    private GameObject InstanceChild(GameObject child)
    {

        //Add child to the parent GridObject
        // Instantiate a copy of the child prefab as a new GameObject
        GameObject newChild = Instantiate(child);

        // Set the parent of the new child GameObject to the parentObject
        newChild.transform.parent = gameObject.transform;

        // Optionally, you can adjust the position and rotation of the new child
        newChild.transform.localRotation = Quaternion.identity;
        return newChild;
    }


    
    public void MoveToTile(Vector3 newPosition)
    {
        // Move the GameObject to the rounded tile position with a specified speed
        // Since origin middle of object, need alterate the object Y (up) position so it doesn't goes in ground
        newPosition.y = _ySize / 2f;
        transform.position = newPosition;

        //if Object was stacked, remove from stack base array
        stackBase?.RemoveStack(this);
        stackBase = null;
    }

    public void StackUp(GridObject baseObject)
    {
        //Called when gridobject hover another grid object instead of a tile

        int stackArrayLength = baseObject.stackArray.Count;

        if(stackArrayLength == baseObject._maxStack-1) { return; } //if reach max stack, return

        Vector3 newPosition = baseObject.transform.position;

        if(baseObject._stackType == StackType.ADDITIVE)
        {
            newPosition.y = stackArrayLength + (_ySize / 2f) + baseObject._ySize;
        }

        transform.position = newPosition;
        stackBase = baseObject;
    }

    private void OnMouseEnter()
    {
        MouseEnter?.Invoke(this);
    }

    private void OnMouseDown()
    {
        //if object is not last stack object => return
        if (stackBase && stackBase.GetLastStackObject() != this )
        {
            return;
        }
        //activate selected object
        this.active = true;
        OnSelected.Invoke(this);
    }

    private void OnMouseUp()
    {
        //deactivate selected objet
        this.active = false;
        MouseUp?.Invoke(this);

        //If stacked => Update stack base array
        stackBase?.AddStack(this);
    }

    public void AddStack(GridObject gridObject)
    {
        stackArray.Add(gridObject);
        Debug.Log(stackArray.Count);
    }

    public void RemoveStack(GridObject gridObject)
    {
        stackArray.Remove(gridObject);
        Debug.Log(stackArray.Count);
    }

    public GridObject GetLastStackObject()
    {
        return stackArray[stackArray.Count-1];
    }


    private void SetColliderSize()
    {
        //Adjust collider size from public parameters
        BoxCollider collider = gameObject.GetComponent<BoxCollider>();
        collider.size = new Vector3(_xSize, _ySize, _zSize);
    }

    private void AdjustColliderSizeFromChildren()
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