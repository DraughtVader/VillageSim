using Pathing;
using UnityEngine;

namespace Jobs
{
    public class WorldObject : MonoBehaviour
    {
        public Point Point { get; protected set; }

        public virtual void OnWorkerInteraction(Worker worker)
        {
            
        }
        
        protected virtual void Awake()
        {
            Point = new Point(transform.position.x, transform.position.y);
        }
    }
}