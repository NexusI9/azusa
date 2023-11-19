using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridObject : MonoBehaviour
{

    public enum StackType
    {
        ADDITIVE, //simply put on top
        EVOLUTIVE //switch instance on stack
    }

    public enum FloorState
    {
        NONE,
        BOTTOM,
        MIDDLE,
        TOP
    }

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
    public GridObject stackBase;
    public List<GridObject> stackArray;
    public int floor = 0; //floor "id"
    FloorState floorState = FloorState.BOTTOM; //flor position (Base/ middle/top)

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
        if (stackBase != null){
            stackBase.RemoveStack(this);
            stackBase.UpdateFloorState();
        }

        //reset to initial state
        floor = 0;
        stackBase = null;
        floorState = FloorState.BOTTOM;
        transform.parent = null;
    }

    public void StackUp(GridObject baseObject)
    {

        //Delegate base object if hovered object already has a base
        if(baseObject.stackBase != null)
        {
            baseObject = baseObject.stackBase;
        }

        //Called when gridobject hover another grid object instead of a tile

        int stackArrayLength = baseObject.stackArray.Count;

        if(stackArrayLength == baseObject._maxStack-1) { return; } //if reach max stack, return

        Vector3 newPosition = baseObject.transform.position;

        if(baseObject._stackType == StackType.ADDITIVE)
        {
            newPosition.y = stackArrayLength + (_ySize / 2f) + baseObject._ySize;
        }

        //1. Update Object Position
        transform.localPosition = newPosition;

        //2. Update Floor State
        //If is currently bottom object => Update stack base array
        if (floorState == FloorState.BOTTOM)
        {
            baseObject.AddStack(this);
            stackBase = baseObject;
            transform.parent = baseObject.gameObject.transform;
            floor = baseObject.stackArray.Count;
            baseObject.UpdateFloorState();
        }


    }

    private void OnMouseEnter()
    {
        MouseEnter?.Invoke(this);
    }

    private void OnMouseDown()
    {

        //if object is not last stack object => return
        if ( stackBase != null && floorState != FloorState.TOP )
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

        GridObjectHelper.GetInfo(this);
    }

    public void AddStack(GridObject gridObject)
    {
        stackArray.Add(gridObject);
        Debug.Log("Add");
    }

    public void RemoveStack(GridObject gridObject)
    {
        stackArray.Remove(gridObject);
        Debug.Log("Remove");
    }

    public FloorState GetFloorState()
    {
        return floorState;
    }

    private void UpdateFloorState()
    {

        //update base floor status
        if (stackBase == null) 
        {
            floorState = FloorState.BOTTOM; 
        }

        //update childs (stacked objects)
        foreach (GridObject stack in stackArray)
        {
            if (stackArray[^1].floor == stack.floor)
            {
                stack.floorState = FloorState.TOP;
            }
            else
            {
                stack.floorState = FloorState.MIDDLE;
            }
        }
        return;


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