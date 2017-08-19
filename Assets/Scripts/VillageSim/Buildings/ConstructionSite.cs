using System.Collections.Generic;
using Buildings;
using UnityEngine;
using UnityEngine.EventSystems;
using VillageSim.Jobs;
using VillageSim.Resources;
using VillageSim.UI;

namespace VillageSim.Buildings
{
	public class ConstructionSite : TimedRegisterObject, IPointerClickHandler, IBuilding
	{
		protected WorldObject buildingPrefab;
		protected ResourceAmount[] buildingResources;
		protected List<ConstructionDropOff> dropOffs;
		protected BuildingInfo buildingInfo;

		public string Name
		{
			get { return buildingInfo.Name; }
		}

		public string Description
		{
			get { return buildingInfo.Description; }
		}

		public ResourceAmount[] ResourcesRequired
		{
			get { return buildingResources; }
		}

		public void SetUp(BuildingInfo info)
		{
			buildingInfo = info;
			buildingPrefab = info.Prefab;
			//we need to clone this otherwise same buildings share resouces
			int length = info.ResourcesRequired.Length;
			buildingResources = new ResourceAmount[length];
			for (int i = 0; i < length; i++)
			{
				buildingResources[i] = new ResourceAmount(info.ResourcesRequired[i]);
			}
			foreach (var buildingResource in buildingResources)
			{
				buildingResource.Current = 0;
			}
		}
		
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

		public List<Collectable.Type> RequiredResources()
		{
			var list = new List<Collectable.Type>();
			foreach (var buildingResource in buildingResources)
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

		public void OnPointerClick(PointerEventData eventData)
		{
			BuildingManagementUi.instance.OpenBuildingInfoPanel(this);
		}

	}
}
