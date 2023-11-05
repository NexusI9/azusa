using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMaster : MonoBehaviour
{

    [Space]

    [SerializeField]
    [Tooltip("Number on width tiles")]
    private int _width = 4;

    [SerializeField]
    [Tooltip("Number of height tiles")]
    private int _height = 4;

    [Space]

    [SerializeField]
    [Tooltip("Tile game objects")]
    private GameObject _tile = null;

    [Space]


    protected static Vector3 lastTilePosition;

    protected int cellSize = 1;
    private int[,] gridArray;

    // Start is called before the first frame update
    private void Start()
    {
        this.gridArray = new int[_width, _height];
        this.Draw();
    }

    private void Update()
    {
    }

    private void Draw()
    {
        for ( int x = 0; x < gridArray.GetLength(0); x++)
        {
            for( int y = 0; y < gridArray.GetLength(1); y++)
            {   
                if (_tile) { DrawTile(x,y,_tile); }
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

    private GameObject DrawTile(int x, int y, GameObject tile)
    {
        GameObject tileInstance = Instantiate(tile, GetWorldPosition(x, y), Quaternion.identity);
        return tileInstance;
    }

}
