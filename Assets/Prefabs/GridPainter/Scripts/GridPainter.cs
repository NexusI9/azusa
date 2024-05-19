using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPainter : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }


    public void MoveTo(Vector3 newPosition)
    {
        newPosition.y = 0.01f;
        transform.position = newPosition;
        //move the grid upward a little to prevent zbuffer issue with tile
    }
}
