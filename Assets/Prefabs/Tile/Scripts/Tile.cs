using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : GridMaster
{

    public bool locked = false;

    public void SetLastTile(Vector3 tilePosition)
    {
        lastTilePosition = tilePosition;
    }

}
