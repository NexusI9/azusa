using System;
using UnityEngine;

namespace Island
{

    public enum BIOTYPE
    {
        CHAOS, //Jungle type, messes everywhere
        CENTRIC, 
        PLAIN, //++ grass &fields, Tree cluster
        ARID //++ rock, --grass, --tree
    }

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

        public Vegetation(Mesh Ar, VegetationItem[] It)
        {
            Area = Ar;
            Items = It;
        }

        public VegetationItem[] Generate()
        {




            return Items;
        }
    }

}
