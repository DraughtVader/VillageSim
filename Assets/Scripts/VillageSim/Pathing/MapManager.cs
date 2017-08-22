using System.Collections.Generic;
using Core.Utilities;
using Pathing;
using UnityEngine;
using Point = Pathing.Point;

namespace VillageSim.Pathing
{
    /// <summary>
    /// A simple console routine to show examples of the A* implementation in use
    /// </summary>
    public class MapManager : Singleton<MapManager>
    {
        [SerializeField]
        protected int width = 10, height = 10;
        
        private bool[,] map;
        private PathFinder pathFinder;

        public List<Point> FindPath(Point startPoint, Point endPoint)
        {
            return pathFinder.FindPath(startPoint, endPoint);
        }

        public void RegisterBlocker(int xPos, int yPos, int sizeX, int sizeY)
        {
            pathFinder.UpdateNodeWalkable(xPos, yPos, false);
            map[xPos, yPos] = false;

            for (int x = 1; x < sizeX; x++)
            {
                pathFinder.UpdateNodeWalkable(x + xPos, yPos, false);
                map[xPos + x, yPos] = false;
            }
            
            for (int y =1; y < sizeY; y++)
            {
                pathFinder.UpdateNodeWalkable(xPos, y + yPos, false);
                map[xPos, yPos + y] = false;
            }
        }
        
        protected override void Awake()
        {
            base.Awake();
            InitializeMap();
            pathFinder = new PathFinder(map);
        }

        private void InitializeMap()
        {
            map = new bool[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    map[x, y] = true;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            for (int x = 0; x <= width; x++)
            {
                Gizmos.DrawLine(new Vector3(x-0.5f, 0, 0), new Vector3(x-0.5f, height, 0));
            }
            for (int y = 0; y <= height; y++)
            {
                Gizmos.DrawLine(new Vector3(0, y-0.5f, 0), new Vector3(width, y-0.5f, 0));
            }

            foreach (var node in pathFinder.Nodes)
            {
                if (!node.IsWalkable)
                {
                    Gizmos.DrawIcon(node.Location.ToVector2(), "Cross.png", true);
                }
            }
        }
    }
}
