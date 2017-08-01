using System;
using System.Collections.Generic;
using Core.Utilities;
using UnityEngine;

namespace Jobs
{
	public class JobManager : Singleton<JobManager>
	{
		protected Dictionary<Collectable.Type, List<Collectable>> collectables = new Dictionary<Collectable.Type, List<Collectable>>();
		protected Dictionary<Collectable.Type, List<HarvestLocation>> harvestLocations = new Dictionary<Collectable.Type, List<HarvestLocation>>();
		protected Dictionary<Collectable.Type, List<DropOffLocation>> dropOffLocations = new Dictionary<Collectable.Type, List<DropOffLocation>>();

		public void RegisterCollectable(Collectable collectable)
		{
			RegisterWorldObject(collectable, collectables, collectable.CollectableType);
		}
		
		public void DeregisterCollectable(Collectable collectable)
		{
			DeregisterWorldObject(collectable, collectables, collectable.CollectableType);
		}
		
		public void RegisterHarvestLocation(HarvestLocation harvestLocation)
		{
			RegisterWorldObject(harvestLocation, harvestLocations, harvestLocation.CollectableType);
		}
		
		public void DeregisterHarvestLocation(HarvestLocation harvestLocation)
		{
			DeregisterWorldObject(harvestLocation, harvestLocations, harvestLocation.CollectableType);
		}
		
		public void RegisterDropOffLocation(DropOffLocation dropOffLocation)
		{
			RegisterWorldObject(dropOffLocation, dropOffLocations, dropOffLocation.CollectableType);
		}
		
		public void DeregisterrDropOffLocation(DropOffLocation dropOffLocation)
		{
			DeregisterWorldObject(dropOffLocation, dropOffLocations, dropOffLocation.CollectableType);
		}

		protected void RegisterWorldObject<T>(T worldObject, Dictionary<Collectable.Type, List<T>> dictionary, Collectable.Type collectableType)
			where T : WorldObject
		{
			if (!dictionary.ContainsKey(collectableType))
			{
				dictionary.Add(collectableType, new List<T>());
			}
			dictionary[collectableType].Add(worldObject);
		}
		
		protected void DeregisterWorldObject<T>(T worldObject, Dictionary<Collectable.Type, List<T>> dictionary, Collectable.Type collectableType)
			where T : WorldObject
		{
			if (dictionary.ContainsKey(collectableType))
			{
				dictionary[collectableType].Remove(worldObject);
			}
		}

		public DropOffLocation GetDropOffLocation(Worker worker)
		{
			return GetAvailableDropOffLocation(worker .HeldItem.CollectableType, worker.transform.position);
		}
		
		public WorldObject GetJob(Worker worker)
		{
			var collectableType = GetResourceForJob(worker.JobType);
			var collectable = GetAvailableCollectable(collectableType, worker.transform.position);
			if (collectable != null)
			{
				collectable.CollectableState = Collectable.State.Targeted;
				return collectable;
			}
			else
			{
				var harvestLocation = GetAvailableHarvestLocation(collectableType, worker.transform.position);
				if (harvestLocation != null)
				{
					return harvestLocation;
				}
			}
			return null;
		}

		private Collectable.Type GetResourceForJob(Job.Type jobType)
		{
			switch (jobType)
			{
				case Job.Type.Lumberjack:
					return Collectable.Type.Wood;
				case Job.Type.Forager:
					return Collectable.Type.Food;
				default:
					throw new ArgumentOutOfRangeException("jobType", jobType, null);
			}
		}

		private Collectable GetAvailableCollectable(Collectable.Type type, Vector3 position)
		{
			if (collectables.ContainsKey(type))
			{
				Collectable closest = null;
				float cloestDistance = float.MaxValue;
				foreach (var collectable in collectables[type])
				{
					if (collectable.CollectableState != Collectable.State.InWorld)
					{
						continue;
					}
					float currentDisstance = Vector3.Distance(collectable.transform.position, position);
					if (currentDisstance < cloestDistance)
					{
						closest = collectable;
						cloestDistance = currentDisstance;
					}
				}
				return closest;
			}
			return null;
		}
		
		private HarvestLocation GetAvailableHarvestLocation(Collectable.Type type, Vector3 position)
		{
			if (harvestLocations.ContainsKey(type))
			{
				HarvestLocation closest = null;
				float cloestDistance = float.MaxValue;
				foreach (var harvestLocation in harvestLocations[type])
				{
					if (!harvestLocation.Harvestable)
					{
						continue;
					}
					float currentDisstance = Vector3.Distance(harvestLocation.transform.position, position);
					if (currentDisstance < cloestDistance)
					{
						closest = harvestLocation;
						cloestDistance = currentDisstance;
					}
				}
				return closest;
			}
			return null;
		}
		
		private DropOffLocation GetAvailableDropOffLocation(Collectable.Type type, Vector3 position)
		{
			if (dropOffLocations.ContainsKey(type))
			{
				DropOffLocation closest = null;
				float cloestDistance = float.MaxValue;
				foreach (var dropOffLocation in dropOffLocations[type])
				{
					if (dropOffLocation.CollectableType != type)
					{
						continue;
					}
					float currentDisstance = Vector3.Distance(dropOffLocation.transform.position, position);
					if (currentDisstance < cloestDistance)
					{
						closest = dropOffLocation;
						cloestDistance = currentDisstance;
					}
				}
				return closest;
			}
			return null;
		}
	}
}	
	