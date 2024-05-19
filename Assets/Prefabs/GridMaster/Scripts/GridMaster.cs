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
    private int[,] gridArray;
    protected GameObject[] tiles;
    protected List<GameObject> gridObjects;


    // Start is called before the first frame update
    private void Start()
    {

        //DrawGrid
        gridArray = new int[_width, _height];
        tiles = new GameObject[_width * _height];
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


    private GridPainter GeneratePainter()
    {
        if(_painter == null) { return null;  }
        GameObject painter = InstantiateAsChild(_painter);
        return painter.GetComponent<GridPainter>();
    }

    private GameObject GenerateTile(GameObject tile, int x, int y)
    {
        //Instantiate object as child and assign relative event/actions logic
        tile = InstantiateAsChild(_tile, new Vector3(x, 0, y));
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


    protected void LockTiles(int xSize, int zSize)
    {
        if (tiles != null)
        {
            foreach (GameObject tile in tiles)
            {
                //Go through each gridobjects and check their position to lock related tiles
                foreach(GameObject gridObject in gridObjects) {
                    bool matchLocation = GridMasterHelper.isWithinBounds(tile, gridObject, xSize, zSize);
                    GridTile tileComponent = tile.GetComponentInChildren<GridTile>();
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
