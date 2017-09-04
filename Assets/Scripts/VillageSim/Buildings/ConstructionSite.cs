using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using VillageSim.Jobs;
using VillageSim.Resources;
using VillageSim.UI;

namespace VillageSim.Buildings
{
	public class ConstructionSite : ResourceRequiringTimedObject, IBuilding, IPointerClickHandler
	{
		protected WorldObject buildingPrefab;
		protected BuildingInfo buildingInfo;
		
		public string Name
		{
			get { return buildingInfo.Name; }
		}

		public string Description
		{
			get { return buildingInfo.Description; }
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			BuildingManagementUi.instance.OpenBuildingInfoPanel(this);
		}
		
		/// <summary>
		/// Returns a required resource for construction
		/// </summary>
		public override Collectable.Type CollectableType
		{
			get
			{
				foreach (var buildingResource in resourcesRequired)
				{
					if (buildingResource.Current < buildingResource.Requirement)
					{
						return buildingResource.Type;
					}
				}
				return Collectable.Type.None;
			}
		}
		
		public void SetUp(BuildingInfo info)
		{
			buildingInfo = info;
			buildingPrefab = info.Prefab;
			//we need to clone this otherwise same buildings share resouces
			int length = info.ResourcesRequired.Length;
			resourcesRequired = new ResourceAmount[length];
			for (int i = 0; i < length; i++)
			{
				resourcesRequired[i] = new ResourceAmount(info.ResourcesRequired[i]);
			}
			foreach (var buildingResource in resourcesRequired)
			{
				buildingResource.Current = 0;
			}
		}

		public override void DropOff(Collectable.Type collectable, ResourceRequiringDropOff dropOff)
		{
			base.DropOff(collectable, dropOff);
			BuildingManagementUi.instance.UpdateBuildingInfo(this);
		}

		protected override void OnWorkComplete()
		{
			base.OnWorkComplete();
			var builtWorldObject = Instantiate(buildingPrefab, transform.position, Quaternion.identity);
			var building = builtWorldObject.GetComponent<Building>();
			if (building != null)
			{
				building.SetUp(buildingInfo);
			}
			foreach (var buildingResource in resourcesRequired)
			{
				var surplus = buildingResource.Current - buildingResource.Requirement;
				if (surplus > 0)
				{
					ResourceManager.instance.DropOffResource(buildingResource.Type, transform.position, surplus);
				}
			}
			Destroy();
		}
	}
}
