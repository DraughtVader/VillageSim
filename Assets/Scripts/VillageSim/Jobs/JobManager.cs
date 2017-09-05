﻿using System;
using System.Collections.Generic;
using Core.Utilities;
using VillageSim.UI;
using UnityEngine;
using VillageSim.Buildings;

namespace VillageSim.Jobs
{
	public class JobManager : Singleton<JobManager>
	{
		public event Action OpenInfoPanel;
		
		[SerializeField]
		protected List<Job> jobs;

		[SerializeField]
		protected WorkerManagementUi workerManagementUi;
		
		protected Dictionary<Collectable.Type, List<Collectable>> collectables = new Dictionary<Collectable.Type, List<Collectable>>();
		protected Dictionary<Collectable.Type, List<HarvestLocation>> harvestLocations = new Dictionary<Collectable.Type, List<HarvestLocation>>();
		protected Dictionary<Collectable.Type, List<DropOffLocation>> dropOffLocations = new Dictionary<Collectable.Type, List<DropOffLocation>>();
		protected Dictionary<Collectable.Type, List<PickUpLocation>> pickUpLocations = new Dictionary<Collectable.Type, List<PickUpLocation>>();
		protected Dictionary<Collectable.Type, List<Refinery>> refineries = new Dictionary<Collectable.Type, List<Refinery>>();
		protected List<ConstructionSite> constructionSites = new List<ConstructionSite>();
		protected List<EnergyRecuperater> energyRecuperaters = new List<EnergyRecuperater>();
		
        public void OpenWorkerInfo(Worker worker)
        {
            workerManagementUi.OpenWorkerInfo(worker);
	        if (OpenInfoPanel != null)
	        {
		        OpenInfoPanel();
	        }
        }
		
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
			var constructionSite = registerable as ConstructionSite;
			if (constructionSite != null)
			{
				constructionSites.Add(constructionSite);
				return;
			}
			var energyRecuperater = registerable as EnergyRecuperater;
			if (energyRecuperater != null)
			{
				energyRecuperaters.Add(energyRecuperater);
				return;
			}
			var pickupLocation = registerable as PickUpLocation;
			if (pickupLocation != null)
			{
				RegisterWorldObject(pickupLocation, pickUpLocations, type);
				return;
			}
			var refinery = registerable as Refinery;
			if (refinery != null)
			{
				RegisterWorldObject(refinery, refineries, refinery.CollectableType);
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
			var constructionSite = registerable as ConstructionSite;
			if (constructionSite != null)
			{
				constructionSites.Remove(constructionSite);
				return;
			}
			var energyRecuperater = registerable as EnergyRecuperater;
			if (energyRecuperater != null)
			{
				energyRecuperaters.Remove(energyRecuperater);
				return;
			}
			var pickupLocation = registerable as PickUpLocation;
			if (pickupLocation != null)
			{
				DeregisterWorldObject(pickupLocation, pickUpLocations, type);
				return;
			}
			var refinery = registerable as Refinery;
			if (refinery != null)
			{
				DeregisterWorldObject(refinery, refineries, refinery.CollectableType);
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
			if (worker.JobType == Job.Type.Builder)
			{
				foreach (ConstructionSite constructionSite in constructionSites)
				{
					if (constructionSite.RequiresResource(type))
					{
						return constructionSite.GetComponent<DropOffLocation>();	
					}
				}
				Debug.LogError("Shits fucked");
				return null;
			}
			if (Job.IsJobRefiner(worker.JobType))
			{
				var allRefineries = refineries[GetResourceForJob(worker.JobType)];
				foreach (var refinery in allRefineries)
				{
					foreach (var resource in refinery.RequiredResources())
					{
						if (resource == type)
						{
							return refinery.GetDropOffLocation(type);
						}
					}
				}	
			}
			return GetAvailable(worker, dropOffLocations, type);
		}
		
		public WorldObject GetCollectableOrPickUpLocation(Worker worker, Collectable.Type type)
		{
			// find collectable
			var collectable = GetAvailable(worker, collectables, type); 
			if (collectable != null)
			{
				collectable.CollectableState = Collectable.State.Targeted;
				return collectable;
			}
			// no collectable, get pickup. Could be null.
			return GetAvailable(worker, pickUpLocations, type);
		}
		
		public void IncreaseJobLimit(Job.Type job, int increase)
		{
			var jobInfo = GetJobInfo(job);
			if (jobInfo != null)
			{
				jobInfo.SupportedWorkers += increase;
			}
		}
		
		public WorldObject GetJob(Worker worker)
		{
			if (CanChangeJob(worker.JobType))
			{
				var currentJob = GetJobInfo(worker.JobType);
				var job = GetMostDesiredJob();
				if (job != null && job.CurrentWorkers < job.SupportedWorkers)
				{
					if (currentJob != null)
					{
						currentJob.CurrentWorkers--;
					}
					worker.AssignJob(job);
					job.CurrentWorkers++;
				}
			}

			if (worker.JobType == Job.Type.Builder)
			{
				// try find construction site
				foreach (var constructionSite in constructionSites)
				{
					if (constructionSite.IsAvailableToWorker(worker))
					{
						return constructionSite;
					}
				}
				// bring resources to construction site
				foreach (var constructionSite in constructionSites)
				{
					var list = constructionSite.RequiredResources();
					var attempted = new List<Collectable.Type>();
					foreach (var requiredResource in list)
					{
						if (!attempted.Contains(requiredResource) && 
						    requiredResource != Collectable.Type.None)
						{
							var resource = GetCollectableOrPickUpLocation(worker, requiredResource);
							if (resource != null)
							{
								return resource;
							}
							// cache these so we dont try getting the same resource
							attempted.Add(requiredResource);
						}
					}
				}
				return null;
			}

			var collectableType = GetResourceForJob(worker.JobType);
			
			if (collectableType == Collectable.Type.None)
			{
				return null;
			}
			var collectable = GetAvailable(worker, collectables, collectableType);
			if (collectable != null)
			{
				//check if worker would be able to drop off collectable first
				var temp = worker.HeldItem;
				worker.HeldItem = collectable;
				var dropOffLocation = GetDropOffLocation(worker, collectableType);
				worker.HeldItem = temp;
				if (dropOffLocation != null)
				{
					collectable.CollectableState = Collectable.State.Targeted;
					return collectable;
				}
			}
			
			if (Job.IsJobRefiner(worker.JobType))
			{
				var refinery = GetAvailable(worker, refineries, collectableType);
				if (refinery != null)
				{
					refinery.AddWorkerEnRouter();
					return refinery;
				}
				var resourceRequiringRefinery = GetRefineryForJob(worker);
				foreach (var resource in resourceRequiringRefinery.ResourcesRequired)
				{
					if (resource.StillRequired > 0)
					{
						var refineryCollectable = GetCollectableOrPickUpLocation(worker, resource.Type);
						return refineryCollectable;
					}
				}
				return null;
			}
			

			var harvestLocation = GetAvailable(worker, harvestLocations, collectableType);
			if (harvestLocation != null)
			{
				harvestLocation.AddWorkerEnRouter();
				return harvestLocation;
			}
			return null;
		}

		public void RemoveWorker(Job.Type jobType)
		{
			var job = GetJobInfo(jobType);
			if (job != null)
			{
				job.CurrentWorkers--;
			}
		}

		public EnergyRecuperater GetEnergyRecuperater(Worker worker)
		{
			foreach (var energyRecuperater in energyRecuperaters)
			{
				if (energyRecuperater.IsAvailableToWorker(worker))
				{
					energyRecuperater.AddWorkerEnRouter();
					return energyRecuperater;
				}
			}
			return null;
		}

		public Sprite GetEnergyRecuperationNeedSprite()
		{
			return workerManagementUi.WorkerEnergyRecuperationNeed;
		}

		private bool CanChangeJob(Job.Type currentJob)
		{
			Job current = GetJobInfo(currentJob);
			if (current == null)
			{
				return true;
			}
			return current.DesiredWorkers < current.CurrentWorkers;
		}

		private Job GetMostDesiredJob()
		{
			int requirement = 0;
			Job mostRequired = null;
			foreach (var job in jobs)
			{
				if (job.CurrentWorkers >= job.SupportedWorkers)
				{
					continue;
				}
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
			BuildingManagementUi.instance.OpenInfoPanel += OnOpenBuildingInfoPanel;
		}

		private void OnOpenBuildingInfoPanel()
		{
			workerManagementUi.CloseWorkerInfo();
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

		private Refinery GetRefineryForJob(Worker worker)
		{
			var appropRefineries = refineries[GetResourceForJob(worker.JobType)];
			return appropRefineries[0];
		}

		public static Collectable.Type GetResourceForJob(Job.Type jobType)
		{
			switch (jobType)
			{
				case Job.Type.Lumberjack:
					return Collectable.Type.Wood;
				case Job.Type.Forager:
					return Collectable.Type.Food;
				case Job.Type.StoneMiner:
					return Collectable.Type.Stone;
				case Job.Type.Carpenter:
					return Collectable.Type.RefinedWood;
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
	