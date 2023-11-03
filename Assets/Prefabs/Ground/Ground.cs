using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
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

    private int cellSize = 1;
    private int[,] gridArray;

    // Start is called before the first frame update
    private void Start()
    {
        this.gridArray = new int[_width, _height];
        this.draw();
    }

    private void draw()
    {
        for ( int x = 0; x < gridArray.GetLength(0); x++)
        {
            for( int y = 0; y < gridArray.GetLength(1); y++)
            {   
                if (_tile) { drawTile(x,y,_tile); }
                else { drawLine(x, y); }
            }
        }

    }

    private Vector3 getWorldPosition(int x, int y)
    {
        return new Vector3(x - _width/2, 0, y- _height/2) * cellSize;
    }

    private void drawLine(int x, int y)
    {
        Debug.DrawLine(getWorldPosition(x, y), getWorldPosition(x + 1, y), Color.white, 100f);
        Debug.DrawLine(getWorldPosition(x, y), getWorldPosition(x, y + 1), Color.white, 100f);
    }

    private void drawTile(int x, int y, GameObject tile)
    {
        Instantiate(tile, getWorldPosition(x,y), Quaternion.identity);
    }

}
