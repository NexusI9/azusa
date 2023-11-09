using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{

    public event System.Action<GameObject> OnKeyPressed;

    public GameObject instance;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {

        if (Input.GetKeyUp(KeyCode.P))
        {
            OnKeyPressed?.Invoke(instance);
        }
        
    }
    
}
