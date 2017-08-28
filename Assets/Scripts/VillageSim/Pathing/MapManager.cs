using System.Collections.Generic;
using Core.Utilities;
using PathFind;
using UnityEngine;

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
        private Grid grid;

        public bool IsNodeWalkable(int x, int y)
        {
            return grid.nodes[x, y].walkable;
        }

        public bool IsAreaWalkable(int xPos, int yPos, int sizeX, int sizeY)
        {
            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    if (!IsNodeWalkable(x + xPos, y + yPos))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        
        public List<Point> FindPath(Point startPoint, Point endPoint)
        {
            return Pathfinding.FindPath(grid, startPoint, endPoint);
        }

        public void SetAreaWalkable(int xPos, int yPos, int sizeX, int sizeY, bool walkable)
        {
            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    grid.nodes[xPos+x, yPos+y].walkable = walkable;
                }
            }
        }
        
        protected override void Awake()
        {
            base.Awake();
            InitializeMap();
            grid = new Grid(width, height, map);
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

            if (Application.isPlaying)
            {
                foreach (var node in grid.nodes)
                {
                    if (node.walkable)
                    {
                        continue;
                    }
                    var pos = new Vector3(node.gridX, node.gridY);
                    Gizmos.DrawIcon(pos, "Cross.png", true);
                }
            }
        }
    }
}
