using UnityEngine;

namespace VillageSim.Pathing
{
	public class PathBlocker : MonoBehaviour
	{
		[SerializeField]
		protected int xSize, ySize;
		
		private void Start () 
		{
			MapManager.instance.SetAreaWalkable((int)transform.position.x,
				(int)transform.position.y, xSize, ySize, false);
		}

		private void OnDestroy()
		{
			if (MapManager.instanceExists)
			{
				MapManager.instance.SetAreaWalkable((int)transform.position.x,
					(int)transform.position.y, xSize, ySize, true);
			}
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			Vector3 bottomLeft = transform.position - new Vector3(0.5f, 0.5f);
			Gizmos.DrawLine(bottomLeft, bottomLeft + new Vector3(xSize, 0));
			Gizmos.DrawLine(bottomLeft, bottomLeft + new Vector3(0, ySize));
			Gizmos.DrawLine(bottomLeft + new Vector3(xSize, ySize), bottomLeft + new Vector3(xSize, 0));
			Gizmos.DrawLine(bottomLeft + new Vector3(xSize, ySize), bottomLeft + new Vector3(0, ySize));
			Gizmos.color = Color.white;
		}
	}
}
