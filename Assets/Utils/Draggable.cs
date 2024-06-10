using System;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    /*
     * Handles object hovering raycasting and moving along cursor freely
     */


    public string HitNameFilter { get; set; } = null;
    public GameObject Target { get; private set; } = null;

    private bool Hovered = false;
    private bool Hooked = false;

    private Vector3 Offset;

    private void Update()
    {
        GameObject hit = Raycaster.GetHitObject();
        if (hit
            && (HitNameFilter != null && hit.name == HitNameFilter)
           )
        {
            Target = hit;
            if (!Hovered)
            {
                OnHover();
                Hovered = true;
            }

            if (Input.GetMouseButtonDown(0)) {
                Hooked = true;
                Offset = Target.transform.parent.position - Raycaster.MouseWorldPosition();
            }
            if (Input.GetMouseButtonUp(0)) {
                Hooked = false;
            }

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
        if(Target == null) { return;  }
        Target.transform.parent.position = Raycaster.MouseWorldPosition() + Offset;
    }


    public void OnHover()
    {
        if (Target == null) { return; }
    }

    public void OnOut()
    {
        if (Target == null) { return; }
    }

}
