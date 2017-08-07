﻿using System.Collections.Generic;
using Core.Utilities;
using Resources;
using UI;
using UnityEngine;

namespace Jobs
{
	public class JobManager : Singleton<JobManager>
	{
		[SerializeField]
		protected List<Job> jobs;

		[SerializeField]
		protected WorkerManagementUi workerManagementUi;

		
		protected Dictionary<Collectable.Type, List<Collectable>> collectables = new Dictionary<Collectable.Type, List<Collectable>>();
		protected Dictionary<Collectable.Type, List<HarvestLocation>> harvestLocations = new Dictionary<Collectable.Type, List<HarvestLocation>>();
		protected Dictionary<Collectable.Type, List<DropOffLocation>> dropOffLocations = new Dictionary<Collectable.Type, List<DropOffLocation>>();
		protected Dictionary<Collectable.Type, List<PickUpLocation>> pickUpLocations = new Dictionary<Collectable.Type, List<PickUpLocation>>();
		
		public void Register(RegisterWorldObject registerable, Collectable.Type type)
		{
			var collectable = registerable as Collectable;
			if (collectable != null)
			{
				RegisterWorldObject(collectable, collectables, type);
				return;
			}
			var harvesteLocation = registerable as HarvestLocation;
			if (harvesteLocation != null)
			{
				RegisterWorldObject(harvesteLocation, harvestLocations, type);
				return;
			}
			var dropoffLocation = registerable as DropOffLocation;
			if (dropoffLocation != null)
			{
				RegisterWorldObject(dropoffLocation, dropOffLocations, type);
				return;
			}
			var pickupLocation = registerable as PickUpLocation;
			if (pickupLocation != null)
			{
				RegisterWorldObject(pickupLocation, pickUpLocations, type);
			}
		}
		
		public void Deregister(RegisterWorldObject registerable, Collectable.Type type)
		{
			var collectable = registerable as Collectable;
			if (collectable != null)
			{
				DeregisterWorldObject(collectable, collectables, type);
				return;
			}
			var harvesteLocation = registerable as HarvestLocation;
			if (harvesteLocation != null)
			{
				DeregisterWorldObject(harvesteLocation, harvestLocations, type);
				return;
			}
			var dropoffLocation = registerable as DropOffLocation;
			if (dropoffLocation != null)
			{
				DeregisterWorldObject(dropoffLocation, dropOffLocations, type);
				return;
			}
			var pickupLocation = registerable as PickUpLocation;
			if (pickupLocation != null)
			{
				DeregisterWorldObject(pickupLocation, pickUpLocations, type);
			}
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

		public DropOffLocation GetDropOffLocation(Worker worker, Collectable.Type type)
		{
			return GetAvailable(worker, dropOffLocations, type);
		}
		
		public WorldObject GetCollectableOrPickUpLocation(Worker worker, Collectable.Type type)
		{
			// find collectable
			var collectable = GetAvailable(worker, collectables, type); 
			if (collectable != null)
			{
				return collectable;
			}
			// no collectable, get pickup. Could be null.
			return GetAvailable(worker, pickUpLocations, type);
		}
		
		public WorldObject GetJob(Worker worker)
		{
			if (CanChangeJob(worker.JobType))
			{
				var currentJob = GetJobInfo(worker.JobType);
				var job = GetMostDesiredJob();
				if (job != null)
				{
					if (currentJob != null)
					{
						currentJob.CurrentWorkers--;
					}
					worker.JobType = job.JobType;
					job.CurrentWorkers++;
				}
			}
			
			var collectableType = GetResourceForJob(worker.JobType);
			if (collectableType == Collectable.Type.None)
			{
				return null;
			}
			var collectable = GetAvailable(worker, collectables, collectableType);
			if (collectable != null)
			{
				collectable.CollectableState = Collectable.State.Targeted;
				return collectable;
			}
			else
			{
				var harvestLocation = GetAvailable(worker, harvestLocations, collectableType);
				if (harvestLocation != null)
				{
					return harvestLocation;
				}
			}
			return null;
		}

		private bool CanChangeJob(Job.Type currentJob)
		{
			Job current = GetJobInfo(currentJob);
			if (current == null)
			{
				return true;
			}
			return current.DesiredWorkers <= current.CurrentWorkers;
		}

		private Job GetMostDesiredJob()
		{
			int requirement = 0;
			Job mostRequired = null;
			foreach (var job in jobs)
			{
				int currentRequirement = job.DesiredWorkers - job.CurrentWorkers;
				if (currentRequirement > requirement)
				{
					requirement = currentRequirement;
					mostRequired = job;
				}
			}
			return mostRequired;
		}

		private void Start()
		{
			workerManagementUi.SetUp(jobs);
		}

		private Job GetJobInfo(Job.Type type)
		{
			foreach (var job in jobs)
			{
				if (job.JobType == type)
				{
					return job;
				}
			}
			return null;
		}

		public static Collectable.Type GetResourceForJob(Job.Type jobType)
		{
			switch (jobType)
			{
				case Job.Type.Lumberjack:
					return Collectable.Type.Wood;
				case Job.Type.Forager:
					return Collectable.Type.Food;
				default:
					return Collectable.Type.None;
			}
		}

		private static T GetAvailable<T>(Worker worker, Dictionary<Collectable.Type, List<T>> dictionary, Collectable.Type collectableType) 
			where T : RegisterWorldObject 
		{
			if (!dictionary.ContainsKey(collectableType))
			{
				return null;
			}
			var list = dictionary[collectableType];
			
			T closest = null;
			var closestDistance = float.MaxValue;
			foreach (var t in list)
			{
				if (!t.IsAvailableToWorker(worker))
				{
					continue;
				}
				float currentDisstance = Vector3.Distance(t.transform.position, worker.transform.position);
				if (currentDisstance < closestDistance)
				{
					closest = t;
					closestDistance = currentDisstance;
				}
			}
			return closest;
		}
	}
}	
	