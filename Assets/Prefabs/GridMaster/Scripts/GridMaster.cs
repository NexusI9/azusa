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

    // Start is called before the first frame update
    private void Start()
    {
        this.gridArray = new int[_width, _height];
        tiles = new GameObject[_width * _height];
        this.DrawGrid();


        //Debug
        objectSpawner = new ObjectSpawner();
        objectSpawner.instance = _spawn;
        objectSpawner.OnKeyPressed += (GameObject child) => { InstantiateAsChild(child); };

    }


    private void DrawGrid()
    {
        for ( int x = 0; x < gridArray.GetLength(0); x++)
        {
            for( int y = 0; y < gridArray.GetLength(1); y++)
            {   
                if (_tile) {
                    int index = x + y * gridArray.GetLength(0); 
                    GameObject tile = InstantiateAsChild(_tile, new Vector3(x, 0, y));
                    Tile tileComponent = tile.GetComponentInChildren<Tile>();
                    tileComponent.TileHover += this.OnTileHover;
                    tiles[index] = tile;
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
                //tile.GetComponent<Tile>().locked = (location == tile.transform.localPosition);
            }

        }

    }

}
