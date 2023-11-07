using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycaster : MonoBehaviour
{

    public static Raycaster Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional, if you want the Raycaster to persist between scenes.
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static GameObject GetHitObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;

            //Return hitObject
            return hitObject;

        }

        return null;

    }

    private void SendMessageTo(GameObject target, string message)
    {
        target.SendMessage(message, SendMessageOptions.DontRequireReceiver);

    }

}
