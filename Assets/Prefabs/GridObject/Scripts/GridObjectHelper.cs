using System;
using System.Collections.Generic;
using UnityEngine;

public class GridObjectHelper
{

    public static void GetInfo(GridObject gridObject)
    {
        Debug.Log(
            "ID:\t"+gridObject.gameObject.GetInstanceID()
            +"\nBase:\t" + gridObject.stackBase
            +"\nFloor Number:\t"+ gridObject.floor
            +"\nFloor State:\t" + gridObject.GetFloorState()
            +"\nBase Stack Array:\t" + gridObject.stackBase?.stackArray.Count
            +"\nNative Stack Array:\t"+ gridObject.stackArray.Count
            + "\n----------\n\n"
            );
    }

}
