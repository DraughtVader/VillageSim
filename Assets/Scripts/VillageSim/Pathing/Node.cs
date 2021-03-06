﻿using System;

namespace Pathing
{
    /// <summary>
    /// Represents a single node on a grid that is being searched for a path between two points
    /// </summary>
    public class Node
    {
        private Node parentNode;

        /// <summary>
        /// The node's location in the grid
        /// </summary>
        public Point Location { get; private set; }

        /// <summary>
        /// True when the node may be traversed, otherwise false
        /// </summary>
        public bool IsWalkable { get; set; }
        
        /// <summary>
        /// Cost from start to here
        /// </summary>
        public float G { get; private set; }

        /// <summary>
        /// Estimated cost from here to end
        /// </summary>
        public float H { get; private set; }

        /// <summary>
        /// Flags whether the node is open, closed or untested by the PathFinder
        /// </summary>
        public NodeState State { get; set; }

        /// <summary>
        /// Estimated total cost (F = G + H)
        /// </summary>
        public float F
        {
            get { return G + H; }
        }

        public Point[] Adjacencies { get; private set; }

        /// <summary>
        /// Gets or sets the parent node. The start node's parent is always null.
        /// </summary>
        public Node ParentNode
        {
            get { return parentNode; }
            set
            {
                // When setting the parent, also calculate the traversal cost from the start node to here (the 'G' value)
                parentNode = value;
                G = parentNode.G + GetTraversalCost(Location, parentNode.Location);
            }
        }

        /// <summary>
        /// Creates a new instance of Node.
        /// </summary>
        /// <param name="x">The node's location along the X axis</param>
        /// <param name="y">The node's location along the Y axis</param>
        /// <param name="isWalkable">True if the node can be traversed, false if the node is a wall</param>
        /// <param name="endLocation">The location of the destination node</param>
        public Node(int x, int y, bool isWalkable, Point endLocation)
        {
            Location = new Point(x, y);
            State = NodeState.Untested;    
            IsWalkable = isWalkable;
            H = GetTraversalCost(Location, endLocation);
            G = 0;
            
            Adjacencies = new[]
            {
                new Point(Location.X-1, Location.Y-1),
                new Point(Location.X-1, Location.Y+1),
                new Point(Location.X+1, Location.Y+1),
                new Point(Location.X+1, Location.Y-1),
                new Point(Location.X-1, Location.Y  ),
                new Point(Location.X,   Location.Y+1),
                new Point(Location.X+1, Location.Y  ),
                new Point(Location.X,   Location.Y-1),
            };
        }
        
        //If this constructor is used SetUpEndNode must be called before the node is used
        public Node(int x, int y, bool isWalkable)
        {
            Location = new Point(x, y);
            IsWalkable = isWalkable;
                            
            Adjacencies = new[]
            {
                new Point(Location.X-1, Location.Y-1),
                new Point(Location.X-1, Location.Y+1),
                new Point(Location.X+1, Location.Y+1),
                new Point(Location.X+1, Location.Y-1),
                new Point(Location.X-1, Location.Y  ),
                new Point(Location.X,   Location.Y+1),
                new Point(Location.X+1, Location.Y  ),
                new Point(Location.X,   Location.Y-1),
            };
        }

        public void SetUpEndNode(Point endLocation)
        {
            State = NodeState.Untested;    
            H = GetTraversalCost(Location, endLocation);
            G = 0;
            parentNode = null;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}: {2}", Location.X, Location.Y, State);
        }

        /// <summary>
        /// Gets the distance between two points
        /// </summary>
        internal static float GetTraversalCost(Point location, Point otherLocation)
        {
            float deltaX = otherLocation.X - location.X;
            float deltaY = otherLocation.Y - location.Y;
            return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }
    }
}
