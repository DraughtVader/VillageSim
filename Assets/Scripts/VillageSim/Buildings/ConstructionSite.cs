using System.Collections.Generic;
using UnityEngine;
using VillageSim.Jobs;
using VillageSim.Resources;

namespace Buildings
{
	public class ConstructionSite : TimedRegisterObject
	{
		[SerializeField]
		protected WorldObject buildingPrefab;

		[SerializeField]
		protected List<ResourceAmount> buildingResources;

		protected List<ConstructionDropOff> dropOffs;
		
		/// <summary>
		/// Returns a required resource for construction
		/// </summary>
		public override Collectable.Type CollectableType
		{
			get
			{
				foreach (var buildingResource in buildingResources)
				{
					if (buildingResource.Current < buildingResource.Requirement)
					{
						return buildingResource.Type;
					}
				}
				return Collectable.Type.None;
			}
		}

		public bool RequiresResource(Collectable.Type type)
		{
			foreach (var buildingResource in buildingResources)
			{
				if (buildingResource.Type == type)
				{
					return buildingResource.Current < buildingResource.Requirement;
				}
			}
			return false;
		}

		protected override void OnWorkComplete()
		{
			Instantiate(buildingPrefab, transform.position, Quaternion.identity);
			foreach (var buildingResource in buildingResources)
			{
				var surplus = buildingResource.Current - buildingResource.Requirement;
				if (surplus > 0)
				{
					ResourceManager.instance.DropOffResource(buildingResource.Type, transform.position, surplus);
				}
			}
			Destroy();
		}

		public override bool IsAvailableToWorker(Worker worker)
		{
			foreach (var resourceAmount in buildingResources)
			{
				if (resourceAmount.Current < resourceAmount.Requirement)
				{
					return false;
				}
			}
			return true;
		}

		public void DropOff(Collectable.Type collectable, ConstructionDropOff dropOff)
		{
			foreach (var buildingResource in buildingResources)
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
				return;
			}
		}

		protected override void Start()
		{
			base.Start();
			dropOffs = new List<ConstructionDropOff>();
			foreach (var resource in buildingResources)
			{
				var dropOff = gameObject.AddComponent<ConstructionDropOff>();
				dropOff.SetUp(resource.Type, this);
				dropOffs.Add(dropOff);
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
	}
}
