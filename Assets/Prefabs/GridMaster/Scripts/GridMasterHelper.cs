using System;
using System.Collections.Generic;
using UnityEngine;

public class GridMasterHelper
{

    public static bool isWithinBounds(GameObject target, GameObject comparator, int xSize, int zSize) {

        Vector3 targetPosition = target.transform.position;
        Vector3 comparatorPosition = comparator.transform.position;


        return
            targetPosition.x >= comparatorPosition.x - xSize/2f &&
            targetPosition.x <= comparatorPosition.x + xSize/2f &&
            targetPosition.z >= comparatorPosition.z - zSize/2f &&
            targetPosition.z <= comparatorPosition.z + zSize/2f;
    }


    public static Vector3 GetWorldPosition(int width, int height, int cellSize,int x, int y)
    {
        return new Vector3(x - width / 2, 0, y - height / 2) * cellSize;
    }

    public static void DrawLine(int width,int height, int cellSize, int x, int y)
    {
        Debug.DrawLine(GetWorldPosition(width, height, cellSize, x, y), GetWorldPosition(width, height, cellSize, x + 1, y), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, height, cellSize, x, y), GetWorldPosition(width, height, cellSize, x, y + 1), Color.white, 100f); ;
    }

}
