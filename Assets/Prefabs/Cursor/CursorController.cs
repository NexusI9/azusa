using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{

    public static CursorController instance;
    public Texture2D _Default, _Active, _Grab, _Type;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void activateDefaultCursor()
    {
        Cursor.SetCursor(_Default, Vector2.zero, CursorMode.Auto);
    }
}
