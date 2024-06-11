using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Island
{
    public class IslandBuilder : MonoBehaviour
    {

        private List<GameObject> Islands = new List<GameObject>();
        public GameObject Island;

        private void Start()
        {


        }

        private void Update()
        {

            if (Input.GetKeyUp(KeyCode.P))
            {
                SpawnIsland();
            }

        }


        private void SpawnIsland() {
            GameObject island = Instantiate(Island, gameObject.transform);
            Islands.Add(island);
        }
    }

}