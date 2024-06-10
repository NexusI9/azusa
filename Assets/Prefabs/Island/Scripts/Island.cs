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
        //Materials
        public Material GroundMaterial;
        public Material RockMaterial;

        //Vegetation
        public int VegetationDensity = 1000;
        public List<GameObject> Vegetation;
        public List<GameObject> Grass;
        public List<GameObject> Tree;
        public List<GameObject> Rock;

        //Mesh
        public List<Chunk> Chunks = new List<Chunk>();
        public Mesh Mesh { get; private set; }

        private void Start()
        {

            gameObject.AddComponent<MeshFilter>();
            gameObject.AddComponent<MeshRenderer>();

            //Add Materials
            gameObject.GetComponent<MeshRenderer>().materials = new Material[] { GroundMaterial, RockMaterial, RockMaterial, RockMaterial };

            Mesh = UpdateMesh();
            gameObject.GetComponent<MeshFilter>().mesh = Mesh;
            Mesh ground = Chunks[0].Circles.Where(c => c.name == "ground").First().mesh;
            GenerateVegetation(ground);
            

        }

        private Mesh UpdateMesh()
        {
            return Chunks.Count == 0 ? BaseChunk() : MergeChunk();
        }

        private Mesh BaseChunk()
        {
            //Generate Base Chunk
            Chunk baseChunk = new Chunk();
            Mesh chunkMesh = baseChunk.Mesh;

            //Add to chunk cache list
            Chunks.Add(baseChunk);

            return chunkMesh;

        }

        
        private Mesh MergeChunk()
        {

            //Edge case if only one chunk
            if(Chunks.Count == 1)
            {
                return Chunks[0].Mesh;
            }

            foreach(Chunk chunk in Chunks)
            {
                
            }

            return Mesh;
        }

        private void GenerateVegetation(Mesh ground)
        {

            //Generate Vegetation

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
                Vegetation baseVegetation = new Vegetation(ground, IslandVegetation, VegetationDensity);
                baseVegetation.Generate();
            }
        }


    }


}

