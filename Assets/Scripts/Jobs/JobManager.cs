using System;
using System.Collections.Generic;
using Core.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Jobs
{
	public class JobManager : Singleton<JobManager>
	{
		[SerializeField]
		protected HarvestLocation[] trees;

		[SerializeField]
		protected DropOffLocation woodPile;
		
		protected Dictionary<Collectable.Type, List<Collectable>> collectables = new Dictionary<Collectable.Type, List<Collectable>>();

		public void RegisterCollectable(Collectable collectable)
		{
			if (!collectables.ContainsKey(collectable.CollectableType))
			{
				collectables.Add(collectable.CollectableType, new List<Collectable>());
			}
			collectables[collectable.CollectableType].Add(collectable);
		}
		
		public void DeregisterCollectable(Collectable collectable)
		{
			if (collectables.ContainsKey(collectable.CollectableType))
			{
				collectables[collectable.CollectableType].Remove(collectable);
			}
		}

		public DropOffLocation GetDropOffLocation(Collectable collectable)
		{
			return woodPile;
		}
		
		public WorldObject GetJob(Worker worker)
		{
			switch (worker.JobType)
			{
				case Job.Type.Lumberjack:
					var collectable = GetAvailableCollectable(Collectable.Type.Wood);
					if (collectable != null)
					{
						collectable.CollectableState = Collectable.State.Targeted;
						return collectable;
					}
					else
					{
						return trees[Random.Range(0, trees.Length)];
					}
				case Job.Type.Forager:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			return null;
		}

		private Collectable GetAvailableCollectable(Collectable.Type type)
		{
			if (collectables.ContainsKey(type))
			{
				foreach (var collectable in collectables[type])
				{
					if (collectable.CollectableState == Collectable.State.InWorld)
					{
						return collectable;
					}
				}
			}
			return null;
		}
	}
}	
	