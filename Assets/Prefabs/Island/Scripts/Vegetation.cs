using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Triangulation;

namespace Island
{

    public enum BIOTOP
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
        private int TotalPoints;
        public float DistanceFromEdge = 1.0f;
        public int AreaVertices = 30;

        public Vegetation(Mesh mesh, VegetationItem[] items, int totalPoints)
        {
            Area = mesh;
            Items = items;
            TotalPoints = totalPoints;

            if(DistanceFromEdge > 0)
            {
                Area = ShrinkMesh();
            }
        }

        public VegetationItem[] Generate(BIOTOP biotop = BIOTOP.CHAOS)
        {

            SpreadPoints spread = new SpreadPoints(Area, TotalPoints);
            spread.Points();

            return Items;
        }

        private Mesh ShrinkMesh()
        {

            //Downsample and simplify shape
            MeshUtils meshUtils = new MeshUtils(Area);
            meshUtils.DownSample(AreaVertices);
            meshUtils.Shrink(DistanceFromEdge);

            //Lost triangulation while downsampling
            Triangulator triangulator = new Triangulator(meshUtils.ToVector2(meshUtils.Mesh.vertices));

            return triangulator.mesh;
        }
    }

}
