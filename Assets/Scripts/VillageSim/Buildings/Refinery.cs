
using UnityEngine;
using VillageSim.Jobs;

namespace VillageSim.Buildings
{
	public class Refinery : ResourceRequiringTimedObject
	{
		[SerializeField]
		protected Collectable productPrefab;
		
		[SerializeField]
		protected int numberOfCollectablesToSpawn;
		
		public override Collectable.Type CollectableType
		{
			get { return productPrefab.CollectableType; }
		}

		protected override void OnWorkComplete()
		{
			for (int i = 0; i < numberOfCollectablesToSpawn; i++)
			{
				Instantiate(productPrefab, transform.position + (Vector3)Random.insideUnitCircle, Quaternion.identity);
			}
		}

		public override void OnWorkerInteraction(Worker worker)
		{
			base.OnWorkerInteraction(worker);

			foreach (var resource in resourcesRequired)
			{
				resource.Current = 0;
			}
		}

	}
}
