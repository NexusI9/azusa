using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMaster : MonoBehaviour
{

    //Parameters
    [Space]

    [SerializeField]
    [Tooltip("Number on width tiles")]
    protected int _width = 4;

    [SerializeField]
    [Tooltip("Number of height tiles")]
    protected int _height = 4;

    [Space]

    [SerializeField]
    [Tooltip("Tile game objects")]
    protected GameObject _tile = null;

    [Space]


    //Private
    protected Tile lastActiveTile;
    protected GridObject lastActiveObject;

    //Debuging
    private ObjectSpawner objectSpawner;

    [SerializeField]
    protected GameObject _spawn = null;

    protected int cellSize = 1;
    private int[,] gridArray;
    protected GameObject[] tiles;
    protected List<GameObject> gridObjects;

    // Start is called before the first frame update
    private void Start()
    {
        this.gridArray = new int[_width, _height];
        tiles = new GameObject[_width * _height];
        gridObjects = new List<GameObject>();
        this.DrawGrid();

        //Debug
        objectSpawner = new ObjectSpawner();
        objectSpawner.instance = _spawn;
        objectSpawner.OnKeyPressed += (GameObject instance) => {
            gridObjects.Add(GenerateGridObject(instance));
        };

    }

    private void Update()
    {
        objectSpawner.Update();
    }


    private void DrawGrid()
    {
        for ( int x = 0; x < gridArray.GetLength(0); x++)
        {
            for( int y = 0; y < gridArray.GetLength(1); y++)
            {   
                if (_tile) {
                    int index = x + y * gridArray.GetLength(0);
                    tiles[index] = GenerateTile(_tile, x, y);
                }
                else { GridMasterHelper.DrawLine(_width, _height, cellSize, x, y); }
            }
        }

    }


    private GameObject GenerateTile(GameObject tile, int x, int y) {
        //Instantiate object as child and assign relative event/actions logic
        tile = InstantiateAsChild(_tile, new Vector3(x, 0, y));
        Tile tileComponent = tile.GetComponentInChildren<Tile>();
        tileComponent.TileHover += this.OnTileHover;
        return tile;
    }

    private GameObject GenerateGridObject(GameObject gridObject)
    {
        //Instantiate object as child and assign relative event/actions logic
        gridObject = InstantiateAsChild(gridObject);
        GridObject gridObjectComponent = gridObject.GetComponent<GridObject>();

        //On Object Selection
        gridObjectComponent.OnSelected += (GridObject selectedObject) =>
        {
            //Set global active Object
            lastActiveObject = selectedObject;
        };

        //On Object Release
        gridObjectComponent.MouseUp += (GridObject selectedObject) =>
        {
            LockTile(selectedObject._xSize, selectedObject._zSize);
            lastActiveObject = null;
        };

        //On Object Hover
        gridObjectComponent.MouseEnter += (GridObject hoveredObject) =>
        {
            if (
                lastActiveObject != null && //if active object exists and being selected
                lastActiveObject != hoveredObject && // if active object is different from hovered object
                lastActiveObject.ID == hoveredObject.ID) // if have same ID => are stackable
            {
                lastActiveObject.StackUp(hoveredObject);
            }
        };


        return gridObject;
    }

    public GameObject InstantiateAsChild(GameObject child, Vector3 position = default)
    {
        // Instantiate the object and set the parent
        GameObject newObject = Instantiate(child, gameObject.transform);
        newObject.transform.localPosition = position;
        newObject.transform.localRotation = Quaternion.identity;

        return newObject;
    }


    private void OnTileHover(Tile tile)
    {
        lastActiveTile = tile;

        //If a grid object is active
        if(lastActiveObject != null)
        {
            //1.Move the last active object

            Vector3 newPosition = tile.transform.position;
            //check if object dimension is pair or not
            int zSize = lastActiveObject._zSize;
            int xSize = lastActiveObject._xSize;

            if (xSize%2 == 0)
            {
                newPosition.x += xSize / (2f*xSize);
            }
            if (zSize%2 == 0)
            { 
                newPosition.z += zSize / (2f*zSize);
            }

           lastActiveObject?.MoveTo(newPosition);

            //2. Check for hovered tiles to make them glow
            foreach(GameObject tl in tiles)
            {
                if( GridMasterHelper.isWithinBounds(tl, lastActiveObject.gameObject, xSize, zSize))
                {
                    //if tile is within active object bounds then make it glow to show potential new location
                    tl.GetComponentInChildren<Tile>().SetGlow(true);
                }
            }
        }
    }


    protected void LockTile(int xSize, int zSize)
    {
        if (tiles != null)
        {
            foreach (GameObject tile in tiles)
            {
                foreach(GameObject gridObject in gridObjects) {
                    bool matchLocation = GridMasterHelper.isWithinBounds(tile, gridObject, xSize, zSize);
                    Tile tileComponent = tile.GetComponentInChildren<Tile>();
                    tileComponent.HandleLockState(matchLocation);
                    if (matchLocation)
                    {
                        tileComponent.SetGlow(false);
                        //skip to next tile if match location is true so doesn't override true state
                        break;
                    }
                }
            }

        }

    }

}
