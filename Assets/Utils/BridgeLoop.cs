using System;
using UnityEngine;
using System.Collections.Generic;

namespace Utils
{

    public class VertiPoint
    {
       public int index { get; set; }
       public Vector3 position { get; set; }
    }

    public class BridgeLoop
    {

        Vector3[] originPoints;
        Vector3[] targetPoints;

        public BridgeLoop(Vector3[] origin, Vector3[] target)
        {
            originPoints = origin;
            targetPoints = target;
        }

        private float Distance(Vector3 A, Vector3 B)
        {
            return Mathf.Sqrt(
                (float) Math.Pow(B.x - A.x, 2)
                + (float) Math.Pow(B.y - A.y, 2)
                + (float) Math.Pow(B.z - A.z, 2)
               );
        }

        private int ClosestVertexIndex(VertiPoint[] group, Vector3[] target)
        {
            Vector3 averagePosition = new Vector3(0,0,0);

            for(int i = 0; i < group.Length; i++)
            {
                averagePosition += group[i].position;
            }

            averagePosition /= group.Length;

            int closestIndex = 0;
            Vector3 closestPosition = Vector3.zero;
            for(int i = 0; i < target.Length; i++)
            {

                if(closestPosition == Vector3.zero
                    || Distance(averagePosition, target[i]) < Distance(averagePosition, closestPosition))
                {
                    closestIndex = i;
                    closestPosition = target[i];
                }
            }

            return closestIndex;
        }

        public Mesh Connect()
        {
            Mesh tempMesh = new Mesh();
            List<Vector3> combinedPoints = new List<Vector3>();

            /* 
             * ===================== VERTICES ===================
             * Simply combine origin and target points
             */

            for (int i = 0; i < originPoints.Length; i++)
            {
                combinedPoints.Add(originPoints[i]);
            }

            for (int j = 0; j < targetPoints.Length; j++)
            {
                combinedPoints.Add(targetPoints[j]);
            }

            //add combined vertices to temporary mesh
            tempMesh.vertices = combinedPoints.ToArray();


            /*
             * ===================== TRIANGLES ===================
             * Segment "Superior" points depending on their proximity to Inferior points
             * 
             * Superior     [0][1][2][3][4]     [5][6][7][8][9]
             *                \__\_|_/__/         \__\_|_/__/
             *                     V                   V
             * Inferior           [0]                 [1] ....
             * 
            */


            Vector3[] superior = originPoints.Length > targetPoints.Length ? originPoints : targetPoints;
            Vector3[] inferior = originPoints.Length > targetPoints.Length ? targetPoints : originPoints;

            //Define points quantity in each groups
            int divider = Mathf.CeilToInt(superior.Length / inferior.Length);

            List<VertiPoint[]> groups = new List<VertiPoint[]>();

            //1. Pack superior into inferior index
            for (int i = 0; i < inferior.Length; i++)
            {
                List<VertiPoint> group = new List<VertiPoint>();
                for (int s = i; s < i + divider; s++)
                {
                    if (s < superior.Length)
                    {
                        group.Add(new VertiPoint()
                        {
                            index = s,
                            position = superior[s]
                        });
                    }

                }

                //Debug.Log($"{i}\t"+string.Join(",", group));
                groups.Add(group.ToArray());
            }

            /*
             * 2. Get the closest vertex index as our starting point (may be 0, but also 9) 
             * and then loop from this starting index point
             * 
             * Group A:
             * 
             *        [0]--x--[1]
             *             |
             *  [10]------[9]---------[8]
             *  
             *  
             *  Group B:
             *  
             *      [0]----[1]--x----[2]
             *                 /
             *                /  
             *  [10]------[9]---------[8]
             *             ------------>
            */

            int startIndex = ClosestVertexIndex(groups[0], inferior);

            /*
             * At this point groups and inferionr have the same length.
             * Now we need to ensure that all our inferior vertex finds their superior group as to prevent holes.
             * 
             * In case our starting index is 6 : 
             * 
             *   [Group 1]   [Group 2]
             *       |           |      ...
             *      [6]         [7]    
             */

            //Go through each of our inferior index
            List<int> triangles = new List<int>();
            for (int i = startIndex; i < inferior.Length + startIndex; i++)
            {
                int realIndex = i % inferior.Length;
                VertiPoint[] currentGroup = groups[i];

                /**
                 * go through each of our groups to plug triangles together
                 * 
                 *    SupIndex NextSupIndex
                 *      [0]--------[1]-------[2]
                 *        \        /      ___/
                 *         \  t1  /  t2__/ 
                 *          \    /  __/    
                 *           \  /__/
                 *           [6]
                 *         realIndex
                 */

                for (int v = 0; v < currentGroup.Length; v++)
                {

                    int SupIndex = v;
                    int NextSupIndex = (v + 1) % currentGroup.Length;

                    triangles.Add(SupIndex);
                    triangles.Add(NextSupIndex);
                    triangles.Add(realIndex);

                    Debug.Log($"{i}\t{currentGroup[v].index}");
                    Debugger.Polygon(new Polygon() {
                        points = new Vector3[]
                        {
                             groups[i][SupIndex].position,
                             inferior[realIndex]
                        }
                    });
                }
            }

        return tempMesh;
        }

    }

}
