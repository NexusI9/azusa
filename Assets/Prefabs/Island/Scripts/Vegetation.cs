using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Island
{

    public enum BIOTYPE
    {
        CHAOS, //Jungle type, messes everywhere
        CENTRIC, 
        PLAIN, //++ grass &fields, Tree cluster
        ARID //++ rock, --grass, --tree
    }

    /*
     *Generate a biotope within a certain area
     *Takes in a list of gameobject along with respective density
     *And spread procedurally the vegetation depending on this density
     */

    public class VegetationItem
    {
        public float Density { get; set; }
        public GameObject[] Objects { get; set; }
        public Vector3[] Position { get; private set; }
    }

    public class Vegetation
    {

        private Mesh Area;
        private VegetationItem[] Items;
        public float DistanceFromEdge = 1.0f;
        public int AreaVertices = 30;

        public Vegetation(Mesh Ar, VegetationItem[] It)
        {
            Area = Ar;
            Items = It;

            if(DistanceFromEdge > 0)
            {
                ShrinkMesh();
            }
        }

        public VegetationItem[] Generate()
        {




            return Items;
        }

        private Mesh ShrinkMesh()
        {

            //Downsample and simplify shape
            MeshUtils meshUtils = new MeshUtils(Area);
            meshUtils.DownSample(AreaVertices);
            meshUtils.Shrink(DistanceFromEdge);


            return meshUtils.Mesh;
        }
    }

}
