using System.Collections.Generic;
using Pathing;
using UnityEngine;

public class Agent : MonoBehaviour
{
	[SerializeField]
	protected float moveSpeed = 1;
	
	[SerializeField]
	protected Point targetPoint;
	
	[SerializeField]
	protected bool isPathing;

	protected MapManager map;
	protected List<Point> path;

	public Point PositionPoint
	{
		get
		{
			return new Point(transform.position.x, transform.position.y);
		}
	}
	
	protected virtual void Update () 
	{
		if (isPathing)
		{
			if (PositionPoint.Equals(targetPoint))
			{
				isPathing = false;
				return;
			}
			path = map.FindPath(PositionPoint, targetPoint);
			Vector3 direction = (path[0] - PositionPoint).Normalized;
			transform.position += direction * Time.deltaTime * moveSpeed;
		}
	}

	protected void Start()
	{
		map = FindObjectOfType<MapManager>();
	}
	
	private void OnDrawGizmosSelected()
	{
		if (path == null || !Application.isPlaying)
		{
			return;
		}
		Gizmos.DrawLine(PositionPoint.ToVector2(), path[0].ToVector2());
		for (int i = 0; i < path.Count - 1; i++)
		{
			Gizmos.DrawLine(path[i].ToVector2(), path[i+1].ToVector2());
		}
	}
}
