using System.Collections.Generic;
using VillageSim.Jobs;
using UnityEngine;

namespace Pathing
{
	public class Agent : MonoBehaviour
	{
		[SerializeField]
		protected float moveSpeed = 1;
	
		protected WorldObject targetObject;
		protected bool isPathing;
		protected MapManager map;
		protected List<Point> path;

		protected Point positionPoint;
		protected Vector3 lastDirection;

		public virtual void MoveTo(WorldObject target)
		{
			if (target == null)
			{
				return;
			}
			targetObject = target;
			if (targetObject.Point == positionPoint)
			{
				OnReachTargetPoint();
				return;
			}
			isPathing = true;
			path = map.FindPath(positionPoint, targetObject.Point);
		}
	
		protected virtual void Update () 
		{
			if (isPathing)
			{
				if (positionPoint.Equals(targetObject.Point))
				{
					isPathing = false;
					OnReachTargetPoint();
					return;
				}
				if (positionPoint.Equals(path[0]))
				{
					path = map.FindPath(positionPoint, targetObject.Point);
				}
				Vector3 direction = (path[0] - positionPoint).Normalized;
				transform.position += direction * Time.deltaTime * moveSpeed;

				if (Mathf.Sign(direction.x) != Mathf.Sign(lastDirection.x))
				{
					var scale = transform.localScale;
					scale.x *= -1;
					transform.localScale = scale;
				}
				lastDirection = direction;
			}

			positionPoint.X = (int)transform.position.x;
			positionPoint.Y = (int)transform.position.y;
		}

		protected virtual void OnReachTargetPoint()
		{
		}

		protected virtual void Awake()
		{
			map = FindObjectOfType<MapManager>();
			positionPoint = new Point(transform.position.x, transform.position.y);
		}
	
		private void OnDrawGizmosSelected()
		{
			if (path == null || !Application.isPlaying)
			{
				return;
			}
			Gizmos.DrawLine(positionPoint.ToVector2(), path[0].ToVector2());
			for (int i = 0; i < path.Count - 1; i++)
			{
				Gizmos.DrawLine(path[i].ToVector2(), path[i+1].ToVector2());
			}
		}
	}
}
