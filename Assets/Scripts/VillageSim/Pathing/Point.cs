using System;
using UnityEngine;

namespace Pathing
{
    [Serializable]
    public class Point
    {
        public int X, Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public Point(float x, float y)
        {
            X = (int)x;
            Y = (int)y;
        }

        public Vector2 Normalized
        {
            get { return (new Vector2(X, Y)).normalized; }
        }

        public override string ToString()
        {
            return string.Format("({0},{1})", X, Y);
        }

        public bool Equals(Point point)
        {
            return (point.X == X && point.Y == Y);
        }
        
        public static Point operator +(Point p1, Point p2)  
        {  
            return new Point(p1.X + p2.X, p1.Y + p2.Y);  
        } 
        
        public static Point operator -(Point p1, Point p2)  
        {  
            return new Point(p1.X - p2.X, p1.Y - p2.Y);  
        }
    }

    public static class ExtensionMethods
    {
        public static Vector2 ToVector2(this Point point)
        {
            return new Vector2(point.X, point.Y);
        }
    }
}