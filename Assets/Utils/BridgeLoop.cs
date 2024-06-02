using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Utils
{

    public class Point
    {
       public int Index { get; set; }
       public Vector3 Position { get; set; }
    }

    public class SuperiorPoint : Point
    {
        public Point Connection { get; set; }
    }

    public class InferiorPoint : Point
    {
        public List<SuperiorPoint> Connections { get; set; }
    }

    public class Edge
    {
        public Point Begin { get; set; }
        public Point End { get; set; }
    }


    public class BridgeLoop
    {

        public List<Edge> Edges { get; private set; } = new List<Edge>();
        public List<Point> Segments { get; private set; } = new List<Point>();
        public List<int> Triangles { get; private set; } = new List<int>();
        public bool DebugMode { get; set; } = false;

        private Vector3[] OriginPoints { get; set; }
        private Vector3[] TargetPoints { get; set; }
        private int SegmentsAmount { get; set; }

        public BridgeLoop(Vector3[] origin, Vector3[] target, int segments = 0)
        {
            OriginPoints = origin;
            TargetPoints = target;
            SegmentsAmount = segments;
        }

        private float Distance(Vector3 A, Vector3 B)
        {
            return Mathf.Sqrt(
                (float) Math.Pow(B.x - A.x, 2)
                + (float) Math.Pow(B.y - A.y, 2)
                + (float) Math.Pow(B.z - A.z, 2)
               );
        }


            private Point ClosestPoint(Vector3 point, Vector3[] target)
        {

            int closestIndex = 0;
            Vector3 closestPosition = Vector3.zero;

            for(int i = 0; i < target.Length; i++)
            {

                if(closestPosition == Vector3.zero
                    || Distance(point, target[i]) < Distance(point, closestPosition))
                {
                    closestIndex = i;
                    closestPosition = target[i];
                }
            }

            return new Point()
            {
                Index = closestIndex,
                Position = closestPosition
            };

        }


        private Mesh Segment(Mesh mesh)
        {

            List<Vector3> segmentsVertices = mesh.vertices.ToList();

            for (int s = 0; s < SegmentsAmount; s++)
            {
                //place points every 1/segments amount
                foreach (Edge edge in Edges)
                {
                    segmentsVertices.Add( edge.Begin.Position + s/SegmentsAmount * (edge.End.Position - edge.Begin.Position) );
                }

            }

            return mesh;

        }



        public Mesh Connect()
        {
            Mesh tempMesh = new Mesh();
            List<Vector3> combinedPoints = new List<Vector3>();

            /* 
             * ===================== VERTICES ===================
             * Simply combine origin and target points
             */

            for (int i = 0; i < OriginPoints.Length; i++)
            {
                combinedPoints.Add(OriginPoints[i]);
            }

            for (int j = 0; j < TargetPoints.Length; j++)
            {
                combinedPoints.Add(TargetPoints[j]);
            }

            //add combined vertices to temporary mesh
            tempMesh.vertices = combinedPoints.ToArray();


            if (DebugMode)
            {

                Debugger.Polygon(new Polygon()
                    {
                        points = tempMesh.vertices,
                        edges = false

                 });

                for (int i = 0; i < tempMesh.vertices.Length; i++)
                {
                    Debugger.Label(new Label()
                    {
                        text = "" + i,
                        position = tempMesh.vertices[i] + new Vector3(0, 1, 0)
                    });
                }
            }


            /*
             * ===================== CLUSTER ===================
             * Segment "Superior" points depending on their proximity to Inferior points
             *             ____________________
             *            |                    |
             * Superior   |  [0][1][2][3][4]   |    [5][6][7][8][9]
             *            |    \__\_|_/__/     |       \__\_|_/__/
             *            |         V          |            V
             * Inferior   |        [0]         |           [1] ....
             *            |____________________|
             *                    Cluster
             * Inferior becomes our Connection Points Target from which the superior points will
             * have to "adapt" themeselves to the available points from inferior
             * 
             * 
             *
            */


            Vector3[] superior = OriginPoints.Length > TargetPoints.Length ? OriginPoints : TargetPoints;
            Vector3[] inferior = OriginPoints.Length > TargetPoints.Length ? TargetPoints : OriginPoints;

            InferiorPoint[] inferiorPoints = new InferiorPoint[inferior.Length];

            /*1. Fill up inferior points array
             * 
             * Note: since we merged our target and origin, the inferior vextices index have been updated,
             * the inferior index have been added to the superior index, 
             * so we need to take this in consideration when assigning the Index value of inferior point
             * that is not anymore "0" but "0 + superior.Length" as instance
             * 
             */

            for (int i = 0; i < inferiorPoints.Length; i++)
            {
                inferiorPoints[i] = new InferiorPoint()
                {
                    Index = superior.Length + i,
                    Position = inferior[i],
                    Connections = new List<SuperiorPoint>()
                };
            }

            //2. First pass to connect Supertior --to--> Inferior depending on their proximity (Harmonisation Phase)
            for(int i = 0; i < superior.Length; i++)
            {

                /*
                 * Get the closest vertex index for each of our points
                 * 
                 *  
                 *      [0]----[1]----[2]
                 *        \    /   ___/
                 *         \  /___/ 
                 *  [10----[9]----------------------[8]
                 *  
                */


                Point connection = ClosestPoint(superior[i], inferior);
                SuperiorPoint verti = new SuperiorPoint()
                {
                    Index = i,
                    Position = superior[i],
                    Connection = connection
                };


                /**
                 * Increment connection number value in our connection points array
                 * as to later on detect if there are orphans or solo connected
                 */

                inferiorPoints[connection.Index].Connections.Add(verti);


            }


            /**
             * 3. Second pass to check orphan inferior points (one that have either 1 or 0 connections)
             * Since unity mesh works with triangle we need to ensure all point have at least 2 connections
             * as to prevent holes in our mesh (Completion Phase)
             * 
             * Note: since merge our target and origin, the vextex index has been updated,
             * the inferior index have been added to the superior index so we
             * need to take this in consideration when looping
             * 
             */

            for(int i = 0; i < inferiorPoints.Length; i++)
            {
                InferiorPoint currentPoint = inferiorPoints[i];

                if (currentPoint.Connections.Count == 0)
                {
                    //Retrieve superior closest point
                    Point closestSuperiorPoint = ClosestPoint(currentPoint.Position, superior );

                    //Add new Superior Point to our Inferior Connection List
                    currentPoint.Connections.Add( new SuperiorPoint() {
                        Index = closestSuperiorPoint.Index,
                        Position = closestSuperiorPoint.Position,
                        Connection = new Point() { Index = currentPoint.Index, Position = currentPoint.Position }
                    });
                }

            }


           /**
            * 4. Automatically link up Inferior Point to previous latest Cluster Superior (n-1)
            * 
            * ]--[3]   [4]---[5]--[6]
            *   /  \__   
            *  /      \_   
            * ]         \      
            *            [2]
            */

            for (int i = 0; i < inferiorPoints.Length; i++)
            {
                InferiorPoint currentPoint = inferiorPoints[i];
                InferiorPoint previousPoint = inferiorPoints[ i == 0 ? inferiorPoints.Length - 1 : i - 1 ];
 
                //Edgecase (NOT RESOLVED)
                if (i == 1 && previousPoint.Connections.Count >= 3)
                {
                    /*
                     * The biggest value of half of the length
                     *
                     */
                    SuperiorPoint maxBelowHalfConnection = previousPoint.Connections[0];
                    foreach(SuperiorPoint sup in previousPoint.Connections)
                    {
        
                        if(sup.Index < superior.Length / 2)
                        {
                            //If current maxBelowHalfConnection Index is above Half, automatically assign this new point that is below half
                            if (maxBelowHalfConnection.Index > superior.Length / 2)
                            {
                                maxBelowHalfConnection = sup;
                            }else if( sup.Index > maxBelowHalfConnection.Index) //Else if maxBelowHalf is below half, check which Index is bigger
                            {
                                maxBelowHalfConnection = sup;
                            }
                        }
                    }

                    currentPoint.Connections.Insert(0, maxBelowHalfConnection);
                }
                else
                {
                    currentPoint.Connections.Insert(0, previousPoint.Connections.Last());
                }

                for (int c = 0; c < currentPoint.Connections.Count; c++)
                {

                    Point currentConnection = currentPoint.Connections[c];

                    //Create edges
                    Edges.Add(new Edge()
                    {
                        Begin = currentConnection,
                        End = currentPoint
                    });

                    //Create Triangle Part A
                    if(c < currentPoint.Connections.Count - 1)
                    {

                        Point nextConnection = currentPoint.Connections[c + 1];
                        //Create Triangle
                        Triangles.Add(currentPoint.Index);
                        Triangles.Add(currentConnection.Index);
                        Triangles.Add(nextConnection.Index);
                    }
               


                    if (DebugMode)
                    {

                    Debugger.Polygon(new Polygon()
                    {
                        points = new Vector3[]{
                            currentPoint.Position,
                            currentConnection.Position
                        }
                    });

                    Debugger.Label(new Label()
                    {
                        text = "" + currentConnection.Index,
                        position = currentConnection.Position + new Vector3(0, 1, 0)
                    });


                    Debugger.Label(new Label()
                    {
                        text = "" + currentPoint.Index,
                        position = currentPoint.Position + new Vector3(0, 1, 0)
                    });
                }


                }

                
                Triangles.Add(currentPoint.Index);
                Triangles.Add(previousPoint.Index);
                Triangles.Add(currentPoint.Connections.First().Index);

            }

            tempMesh.triangles = Triangles.ToArray();


            //Generate uvs (cylindrical projections
            Uv uv = new Uv();
            tempMesh.uv = uv.Cylindrical(tempMesh.vertices);

            //Generate normals
            Normal normal = new Normal();
            tempMesh.normals = normal.Set(tempMesh);

            tempMesh.RecalculateNormals();

            //Segment connections into strats
            //tempMesh = Segment(tempMesh);

            return tempMesh;
        }

    }

}
