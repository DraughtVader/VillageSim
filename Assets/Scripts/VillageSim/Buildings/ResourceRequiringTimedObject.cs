using System;
using System.Collections.Generic;
using UnityEngine;
using VillageSim.Jobs;
using VillageSim.Resources;

namespace VillageSim.Buildings
{
	public abstract class ResourceRequiringTimedObject : TimedRegisterObject
	{
		public event Action DroppedOff;
		public event Action TimeComplete;
		
		protected List<ResourceRequiringDropOff> dropOffs;
		
		[SerializeField]
		protected ResourceAmount[] resourcesRequired;

		public ResourceAmount[] ResourcesRequired
		{
			get { return resourcesRequired; }
		}

		public List<Collectable.Type> RequiredResources()
		{
			var list = new List<Collectable.Type>();
			foreach (var buildingResource in resourcesRequired)
			{
				if (buildingResource.Current < buildingResource.Requirement)
				{
					list.Add(buildingResource.Type);
				}
			}
			return list;
		}

		public bool RequiresResource(Collectable.Type type)
		{
			foreach (var buildingResource in resourcesRequired)
			{
				if (buildingResource.Type == type)
				{
					return buildingResource.Current < buildingResource.Requirement;
				}
			}
			return false;
		}

		public override bool IsAvailableToWorker(Worker worker)
		{
			foreach (var resourceAmount in resourcesRequired)
			{
				if (resourceAmount.Current < resourceAmount.Requirement)
				{
					return false;
				}
			}
			return true;
		}
		
		public DropOffLocation GetDropOffLocation(Collectable.Type collectableType)
		{
			foreach (var dropOff in dropOffs)
			{
				if (dropOff.CollectableType == collectableType)
				{
					return dropOff;
				}
			}
			return null;
		}

		public virtual void DropOff(Collectable.Type collectable, ResourceRequiringDropOff dropOff)
		{

			foreach (var buildingResource in resourcesRequired)
			{
				if (buildingResource.Type != collectable)
				{
					continue;
				}
				buildingResource.Current++;

				if (buildingResource.Current == buildingResource.Requirement)
				{
					dropOff.Complete();
				}
				if (DroppedOff != null)
				{
					DroppedOff();
				}
				return;
			}
		}

		protected override void OnWorkComplete()
		{
			if (TimeComplete != null)
			{
				TimeComplete();
			}		
		}

		protected override void Destroy()
		{
			foreach (var dropOff in dropOffs)
			{
				JobManager.instance.Deregister(dropOff, dropOff.CollectableType);
			}
			base.Destroy();
		}
		
		protected override void Start()
		{
			base.Start();
			dropOffs = new List<ResourceRequiringDropOff>();
			foreach (var resource in resourcesRequired)
			{
				var dropOff = gameObject.AddComponent<ResourceRequiringDropOff>();
				dropOff.SetUp(resource.Type, this);
				dropOffs.Add(dropOff);
			}
		}
	}
}
