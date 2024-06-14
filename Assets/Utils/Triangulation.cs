﻿using System.Collections.Generic;
using UnityEngine;
using DelaunatorSharp;
using Utils;

/*
 Execute a Delaunay Triangulation
 but also remove the useless triangles that fall out of the perimeter
 */
namespace Triangulation
{

    public class Triangulator {

        private enum CleanTriangleMethod
        {
            RAYCAST,
            HULL
        };


        public Mesh Mesh { get; private set; }
        public Vector3[] Vertices { get; private set; }
        public int[] Triangles { get; private set; }

        private Vector2[] Points { get; set; }
        public Delaunator Delaunator { get; set; }

        private Vector2[] OuterEdges { get; set; }

        public Triangulator(Vector2[] initPoints, Vector2[] outerEdges = null) {

            OuterEdges = outerEdges == null ? initPoints : outerEdges;

            IPoint[] pts = GeneratePoints(initPoints);
            Points = initPoints;

            Delaunator = new Delaunator(pts);

            Mesh = new Mesh();
            Mesh.vertices = GenerateVertices();
            Mesh.triangles = GenerateTriangles(CleanTriangleMethod.RAYCAST);

            Vertices = Mesh.vertices;
            Triangles = Mesh.triangles;

        }

        private IPoint[] GeneratePoints(Vector2[] points)
        {
            List<IPoint> pts = new List<IPoint>();

            for (int i = 0; i < points.Length; i++)
            {
                Vector2 point = points[i];
                pts.Add(new DelaunatorSharp.Point() { X = point.x, Y = point.y });
            }

            return pts.ToArray();
        }


        private Vector3[] GenerateVertices()
        {

            List<Vector3> vert = new List<Vector3>();

            foreach (IPoint pt in Delaunator.Points)
            {
                //Get points stored in triangles
                vert.Add(new Vector3((float)pt.X, 0, (float)pt.Y));
            }

            return vert.ToArray();
        }



        private int[] GenerateTriangles(CleanTriangleMethod method)
        {
            return method == CleanTriangleMethod.RAYCAST ? CleanTrianglesRaycast(Delaunator.GetTriangles()) : CleanTrianglesHull(Delaunator.GetTriangles());
        }

        private int[] CleanTrianglesRaycast(IEnumerable<ITriangle> triangles)
        {

            List<int> cleanTri = new List<int>();

            foreach (ITriangle tri in triangles)
            {

                //Get centroid Point coordinate of triangle
                IPoint centroid = Delaunator.GetCentroid(tri.Index);

                //Go through each points of our initial shape and check the intersection number via raycasting vector
                int nIntersection = 0;
                for (int pt = 0; pt < OuterEdges.Length; pt++)
                {

                    //Get current and new point to define our vector
                    Vector2 currentPoint = OuterEdges[pt];
                    Vector2 nextPoint = OuterEdges[(pt + 1) % OuterEdges.Length];

                    //Convert vector to point
                    DelaunatorSharp.Point sideStart = new() { X = currentPoint.x, Y = currentPoint.y };
                    DelaunatorSharp.Point sideEnd = new() { X = nextPoint.x, Y = nextPoint.y };

                    //Check if centroid is in or out via raycasting
                    if (IsIntersecting(centroid, sideStart, sideEnd)) nIntersection++;
                }

                if ((nIntersection & 1) == 1)
                {

                    //Inside, append triangle indexes
                    foreach (int index in Delaunator.PointsOfTriangle(tri.Index)) {
                        cleanTri.Add(index);
                    }

                }

            }

            return cleanTri.ToArray();
        }

        private int[] CleanTrianglesHull(IEnumerable<ITriangle> triangles)
        {
            List<int> cleanTri = new List<int>();

            foreach (ITriangle tri in triangles)
            {
                if(BelongToHull(tri.Points, Delaunator.GetHullPoints()) == false)
                {
                    //Inside, append triangle indexes
                    foreach (int index in Delaunator.PointsOfTriangle(tri.Index))
                    {
                        cleanTri.Add(index);
                    }
                }
            }

            return cleanTri.ToArray();
        }

        private bool BelongToHull(IEnumerable<IPoint> points, IPoint[] hull)
        {
            /*
             * Detect if at least two points belongs to Hull
             * If belongs to hull then it means it's out of the shape
             */


            int commonPoints = 0;

            foreach (IPoint point in points)
            {
                for(int i = 0; i < hull.Length; i++)
                {
                    IPoint currentHullPoint = hull[i];

                    if( point.X == currentHullPoint.X && point.Y == currentHullPoint.Y)
                    {
                        commonPoints++;
                    }

                }

            }

            return commonPoints >= 2;
        }

        private bool IsIntersecting(IPoint point, IPoint sideStart, IPoint sideEnd)
        {

            //Ray
            float v1x1 = (float) point.X;
            float v1y1 = (float) point.Y;
            float v1x2 = 0.0f;
            float v1y2 = 100.0f;

            //Side
            float v2x1 = (float) sideStart.X;
            float v2y1 = (float) sideStart.Y;
            float v2x2 = (float) sideEnd.X;
            float v2y2 = (float) sideEnd.Y;

            float d1, d2;
            float a1, a2, b1, b2, c1, c2;

            // Convert vector 1 to a line (line 1) of infinite length.
            // We want the line in linear equation standard form: A*x + B*y + C = 0
            // See: http://en.wikipedia.org/wiki/Linear_equation
            a1 = v1y2 - v1y1;
            b1 = v1x1 - v1x2;
            c1 = (v1x2 * v1y1) - (v1x1 * v1y2);

            // Every point (x,y), that solves the equation above, is on the line,
            // every point that does not solve it, is not. The equation will have a
            // positive result if it is on one side of the line and a negative one 
            // if is on the other side of it. We insert (x1,y1) and (x2,y2) of vector
            // 2 into the equation above.
            d1 = (a1 * v2x1) + (b1 * v2y1) + c1;
            d2 = (a1 * v2x2) + (b1 * v2y2) + c1;

            // If d1 and d2 both have the same sign, they are both on the same side
            // of our line 1 and in that case no intersection is possible. Careful, 
            // 0 is a special case, that's why we don't test ">=" and "<=", 
            // but "<" and ">".
            if (d1 > 0 && d2 > 0) return false;
            if (d1 < 0 && d2 < 0) return false;

            // The fact that vector 2 intersected the infinite line 1 above doesn't 
            // mean it also intersects the vector 1. Vector 1 is only a subset of that
            // infinite line 1, so it may have intersected that line before the vector
            // started or after it ended. To know for sure, we have to repeat the
            // the same test the other way round. We start by calculating the 
            // infinite line 2 in linear equation standard form.
            a2 = v2y2 - v2y1;
            b2 = v2x1 - v2x2;
            c2 = (v2x2 * v2y1) - (v2x1 * v2y2);

            // Calculate d1 and d2 again, this time using points of vector 1.
            d1 = (a2 * v1x1) + (b2 * v1y1) + c2;
            d2 = (a2 * v1x2) + (b2 * v1y2) + c2;

            // Again, if both have the same sign (and neither one is 0),
            // no intersection is possible.
            if (d1 > 0 && d2 > 0) return false;
            if (d1 < 0 && d2 < 0) return false;

            // If we get here, only two possibilities are left. Either the two
            // vectors intersect in exactly one point or they are collinear, which
            // means they intersect in any number of points from zero to infinite.
            if ((a1 * b2) - (a2 * b1) == 0.0f) return false; // collinear

            // If they are not collinear, they must intersect in exactly one point.
            return true;
        }

    }

}