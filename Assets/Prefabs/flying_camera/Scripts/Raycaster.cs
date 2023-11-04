using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycaster : MonoBehaviour
{

    protected virtual void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject)
            {
                OnCursorEnter(hitObject, hit);
            }
        }
        else
        {
            OnCursorLeave(hit);
        }
    }

    protected virtual void OnCursorEnter(GameObject hitObject, RaycastHit hit)
    {
        
    }

    protected virtual void OnCursorLeave(RaycastHit hit)
    {

    }
}
