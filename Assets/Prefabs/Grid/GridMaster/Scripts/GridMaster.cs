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
    [Tooltip("Number of length tiles")]
    protected int _length = 4;

    [SerializeField]
    [Tooltip("Number of max height tiles")]
    protected int _height = 4;

    [Space]

    [SerializeField]
    [Tooltip("Tile game objects")]
    protected GameObject _tile = null;

    [Space]


    [SerializeField]
    protected GameObject _painter = null;

    [Space]

    //Global Children
    protected static GridTile lastActiveTile;
    protected static GridObject lastActiveObject;
    protected static GridPainter gridPainter;

    //Debuging
    private ObjectSpawner objectSpawner;

    [SerializeField]
    protected GameObject _spawn = null;

    protected int cellSize = 1;
    protected GameObject[,,] tiles;
    protected List<GameObject> gridObjects;


    // Start is called before the first frame update
    private void Start()
    {

        //DrawGrid
        tiles = new GameObject[_width, _height, _length];
        gridObjects = new List<GameObject>();
        DrawGrid();

        //GridPainter
        gridPainter = GeneratePainter();


        //Debug
        objectSpawner = new ObjectSpawner();
        objectSpawner.instance = _spawn;
        objectSpawner.OnKeyPressed += (GameObject instance) => {
            gridObjects.Add(GenerateGridObject(instance));
        };

    }

    private void Update()
    {
        //objectSpawner.Update();
    }


    private void DrawGrid()
    {
        for ( int x = 0; x < tiles.GetLength(0); x++)
        {
            for( int y = 0; y < tiles.GetLength(1); y++)
            {
                for (int z = 0; z < tiles.GetLength(2); z++)
                {
                    if (_tile)
                    {
                        //store tile in array for fututre referencing and checking
                        tiles[x,y,z] = GenerateTile(_tile, x, y, z);
                    }
                    else { GridMasterHelper.DrawLine(_width, _length, cellSize, x, z); }
                }
            }
        }
    }


    private GridPainter GeneratePainter()
    {
        if(_painter == null) { return null;  }
        GameObject painter = InstantiateAsChild(_painter);
        return painter.GetComponent<GridPainter>();
    }

    private GameObject GenerateTile(GameObject tile, int x, int y, int z)
    {
        //instantiate object as child
        tile = InstantiateAsChild(tile, new Vector3(x, y, z));

        //set first level tiles active by default
        GridTile tileComponent = tile.GetComponentInChildren<GridTile>();
        tileComponent.SetActive(y == 0);
        tileComponent.SetLevel(y);
        return tile;
    }

    private GameObject GenerateGridObject(GameObject gridObject)
    {
        //Instantiate object as child and assign relative event/actions logic
        gridObject = InstantiateAsChild(gridObject);
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

    protected void LockTile(bool locked, int x, int y, int z)
    {
        //target tile in array
        GameObject targetTile = tiles[x, y, z];
        if (targetTile)
        {
            //if exists lock this specific tile
            GridTile tileComponent = targetTile.GetComponentInChildren<GridTile>();
            //lock the tile or not if there is a gridobject at this position
            tileComponent.setLocked(locked);
        }
    }

}
