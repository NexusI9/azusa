using System;
using System.Linq;
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
        public Vector3[] Position { get; set; }
    }

    public class Vegetation
    {

        private Mesh Area;
        private VegetationItem[] Items;
        private int TotalPoints;
        public float DistanceFromEdge = 1.0f;
        public int AreaVertices = 30;
        public bool DebugMode = false;

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
            List<Vector3> points = spread.Points.ToList();
            List<int> availableIndex = Enumerable.Range(0,points.Count - 1).ToList();
            List<VegetationItem> enumItems = Items.ToList();

            float sumDensity = 0.0f;
            enumItems.ForEach(item  => sumDensity += item.Density);
             
            //assing points randomly in function of density
            foreach (VegetationItem item in enumItems)
            {
                float densityToNumber = Mathf.FloorToInt(item.Density * points.Count / sumDensity);
                List<Vector3> positions = new List<Vector3>();

                for(int n = 0; n < densityToNumber; n++)
                {
                    int randomIndex = UnityEngine.Random.Range(0, availableIndex.Count - 1);
                    //Add to item array
                    positions.Add( points[randomIndex] );

                    //Remove selected indexes from available Index
                    availableIndex.RemoveAt(randomIndex);
                }

                item.Position = positions.ToArray();
            }


            //Debug
            if (DebugMode)
            {
                Color[] colors = new Color[] {
                Color.blue,
                Color.red,
                Color.white,
                Color.magenta,
                Color.yellow,
                Color.cyan
            };


                for (int i = 0; i < enumItems.Count; i++)
                {
                    for (int p = 0; p < enumItems[i].Position.Length; p++)
                    {
                        Debugger.Cube(new Cube()
                        {
                            Position = enumItems[i].Position[p],
                            Size = new Vector3(0.3f, 0.3f, 0.3f),
                            Color = colors[i % colors.Length] 
                        });
                    }
                }
            }


            return enumItems.ToArray();
        }

        private Mesh ShrinkMesh()
        {

            //Downsample and simplify shape
            Mesh shrinkMesh = MeshUtils.DownSample(Area, AreaVertices);
            shrinkMesh = MeshUtils.Shrink(shrinkMesh, DistanceFromEdge);

            //Lost triangulation while downsampling
            Triangulator triangulator = new Triangulator(MeshUtils.ToVector2(shrinkMesh.vertices));

            return triangulator.Mesh;
        }
    }

}
