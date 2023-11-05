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

            //Hover event
            if (hitObject)
            {
                OnCursorEnter(hitObject);

                //Click event
                if (Input.GetMouseButtonDown(0))
                {
            
                    OnCursorDown(hitObject);
                }

                if (Input.GetMouseButtonUp(0))
                {
                    OnCursorUp(hitObject);
                }
            }
            else
            {
                OnCursorLeave(hitObject);
            }

        }

    }

    protected virtual void OnCursorEnter(GameObject hitObject)
    {
        
    }

    protected virtual void OnCursorLeave(GameObject hitObject)
    {

    }

    protected virtual void OnCursorDown(GameObject hitObject)
    {

    }

    protected virtual void OnCursorUp(GameObject hitObject)
    {

    }

    protected bool IsTargeted(GameObject gameObject, GameObject hitObject)
    {
        return GameObject.ReferenceEquals(gameObject, hitObject);

    }
}
