using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pathing
{
    /// <summary>
    /// A simple console routine to show examples of the A* implementation in use
    /// </summary>
    public class MapManager : MonoBehaviour
    {
        [SerializeField]
        protected int width = 10, height = 10;

        [SerializeField]
        protected GameObject startPrefab, endPrefab, blockerPrefab, routeMarkerPrefab;

        [SerializeField]
        protected List<Point> barriers;
        
        private bool[,] map;
        private PathFinder pathFinder;

        public List<Point> FindPath(Point startPoint, Point endPoint)
        {
            return pathFinder.FindPath(startPoint, endPoint, map);
        }
        
        private void Awake()
        {
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

            foreach (var barrier in barriers)
            {
                map[barrier.X, barrier.Y] = false;
                Instantiate(blockerPrefab, new Vector2(barrier.X, barrier.Y), Quaternion.identity);
            }
        }

        private void OnDrawGizmosSelected()
        {
            for (int x = 0; x <= width; x++)
            {
                Gizmos.DrawLine(new Vector3(x, 0, 0), new Vector3(x, height, 0));
            }
            for (int y = 0; y <= height; y++)
            {
                Gizmos.DrawLine(new Vector3(0, y, 0), new Vector3(width, y, 0));
            }
        }
    }
}
