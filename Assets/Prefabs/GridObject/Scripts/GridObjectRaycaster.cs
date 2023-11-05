using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObjectRaycaster : Raycaster
{

    protected override void OnCursorDown(GameObject hitObject)
    {

        if (this.IsTargeted(gameObject, hitObject))
        {
            //activate selected object
            gameObject.GetComponentInParent<GridObject>().active = true;
        }

    }

    protected override void OnCursorUp(GameObject hitObject)
    {
        if (this.IsTargeted(gameObject, hitObject))
        {
            //deactivate selected objet
            gameObject.GetComponentInParent<GridObject>().active = false;
        }
    }


}
