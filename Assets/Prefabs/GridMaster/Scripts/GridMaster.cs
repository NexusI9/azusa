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
                else { DrawLine(x, y); }
            }
        }

    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x - _width/2, 0, y- _height/2) * cellSize;
    }

    private void DrawLine(int x, int y)
    {
        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
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
        gridObjectComponent.OnSelected += (GridObject selectedObject) =>
        {
            lastActiveObject = selectedObject;
        };

        gridObjectComponent.MouseUp += (GridObject selectedObject) =>
        {
            LockTileAt(selectedObject.transform.position, selectedObject._xLength, selectedObject._zLength);
            lastActiveObject = null;
        };


        return gridObject;
    }


    private GameObject InstantiateAsChild(GameObject child, Vector3 position=default)
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
        lastActiveObject?.MoveTo(tile.transform.position);
    }

    private void OnObjectDown(GridObject gridObject)
    {


    }

    protected void LockTileAt(Vector3 location, int xLength, int yLength)
    {
        if (tiles != null)
        {
            foreach (GameObject tile in tiles)
            {
                foreach(GameObject gridObject in gridObjects) {
                    bool matchLocation = gridObject.transform.localPosition == tile.transform.localPosition;
                    tile.GetComponentInChildren<Tile>().HandleLockState(matchLocation);
                    if (matchLocation)
                    {
                        //skip to next tile if match location is true so doesn't override true state
                        break;
                    }
                }
            }

        }

    }

}
