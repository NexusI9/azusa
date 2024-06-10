using System;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    /*
     * Handles object hovering raycasting and moving along cursor freely
     */


    public string HitNameFilter = null;
    private bool Hovered = false;
    private bool Hooked = false;
    private Vector3 Offset;

    private void Update()
    {
        GameObject hit = Raycaster.GetHitObject();
        if (hit && (HitNameFilter != null && hit.name == HitNameFilter) )
        {
            if (!Hovered)
            {
                OnHover();
                Hovered = true;
            }

            if (Input.GetMouseButtonDown(0)) {
                Hooked = true;
                Offset = gameObject.transform.position - Raycaster.MouseWorldPosition();
            }
            if (Input.GetMouseButtonUp(0)) Hooked = false;

            if (Hooked) {
                OnMove();
             }
            

        }
        else
        {
            if (Hovered)
            {
                Hovered = false;
                OnOut();
            }
        }
    }

    public void OnMove()
    {
        gameObject.transform.position = Raycaster.MouseWorldPosition() + Offset;
    }


    public void OnHover()
    {

    }

    public void OnOut()
    {

    }

}
