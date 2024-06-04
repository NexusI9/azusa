using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

/*
 Islands are a gathering of chunks
 */

namespace Island
{

    public class Island : MonoBehaviour
    {

        private List<Chunk> Chunks = new List<Chunk>();
        public Material GroundMaterial;
        public Material RockMaterial;

        public List<GameObject> Vegetation;
        public List<GameObject> Grass;
        public List<GameObject> Tree;
        public List<GameObject> Rock;

        private void Start()
        {

            gameObject.AddComponent<MeshFilter>();
            gameObject.AddComponent<MeshRenderer>();

            //Add Materials
            gameObject.GetComponent<MeshRenderer>().materials = new Material[] { GroundMaterial, RockMaterial, RockMaterial, RockMaterial };


            //Generate Base Chunk
            Chunk baseChunk = new Chunk();
            gameObject.GetComponent<MeshFilter>().mesh = baseChunk.Spawn(new Vector2(0, 0));

            //Generate Vegetation
            Mesh ground = baseChunk.Circles.Where(c => c.name == "ground").First().mesh;

            //Vegetation Config
            VegetationItem[] IslandVegetation = new VegetationItem[]
            {
                new VegetationItem()
                {
                    Objects = Vegetation.ToArray(),
                    Density = 0.7f
                },
                new VegetationItem()
                {
                    Objects = Grass.ToArray(),
                    Density = 1.0f
                },
                new VegetationItem()
                {
                    Objects = Tree.ToArray(),
                    Density = 0.5f
                },
                new VegetationItem()
                {
                    Objects = Rock.ToArray(),
                    Density = 0.1f
                }
            };

            if (ground)
            {
                Vegetation baseVegetation = new Vegetation(ground, IslandVegetation);
            }

        }

    }


}

