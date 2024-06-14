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
        public class ChunkCollider
        {
            public Chunk Chunk { get; set; }
            public GameObject Collider { get; set; }
        }

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
        public List<ChunkCollider> ChunkColliders = new List<ChunkCollider>();
        private const string CHUNK_COLLIDER_NAME = "ChunkCollider";
        public Mesh Mesh { get; private set; }

        private void Start()
        {
            gameObject.AddComponent<MeshRenderer>();
            gameObject.AddComponent<MeshFilter>();

            //Inheritance from Draggable
            Draggable draggable = gameObject.GetComponent<Draggable>();
            draggable.HitNameFilter = CHUNK_COLLIDER_NAME;

            //Add Materials
            gameObject.GetComponent<MeshRenderer>().materials = new Material[] { GroundMaterial, RockMaterial, RockMaterial, RockMaterial };

            Mesh = UpdateMesh();
            UpdateBounds();
            gameObject.GetComponent<MeshFilter>().mesh = Mesh;

            Mesh ground = ChunkColliders[0].Chunk.Circles.Where(c => c.name == "ground").First().Mesh;
            GenerateVegetation(ground); 
        }

        private Mesh UpdateMesh()
        {

            //Generate Base Chunk
            if (ChunkColliders.Count == 0)
            {
                Chunk baseChunk = new Chunk();
                Mesh chunkMesh = baseChunk.Mesh;

                //Add to chunk cache list
                ChunkColliders.Add(new ChunkCollider() { Chunk = baseChunk });
                return chunkMesh;
            }

            //Assign Solo Chunk
            if (ChunkColliders.Count == 1)
            {
                return ChunkColliders[0].Chunk.Mesh;
            }

            //Merge chunks
            foreach (ChunkCollider chunkCollider in ChunkColliders)
            {

            }

            return Mesh;
        }

        private void UpdateBounds()
        {

            //Purge


            //Add Collider as Children as Unity can by default only handle 1 collider / gameobjects
            foreach(ChunkCollider chunkCollider in ChunkColliders)
            { 
                GameObject collider = new GameObject(CHUNK_COLLIDER_NAME);
                collider.transform.parent = this.gameObject.transform;
                collider.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(Vector3.zero) ) ;
                BoxCollider boxCollider = collider.AddComponent<BoxCollider>();
                boxCollider.center = chunkCollider.Chunk.Bounds.center;
                boxCollider.size = chunkCollider.Chunk.Bounds.size;
                
                chunkCollider.Collider = collider;
            }
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

