using System;
using UnityEngine;
using UnityEngine.EventSystems;
using VillageSim.Resources;
using VillageSim.UI;

namespace VillageSim.Buildings
{
	public class Building : MonoBehaviour, IBuilding, IPointerClickHandler
	{
		[SerializeField]
		protected string buildingName;
		
		public string Name
		{
			get { return buildingInfo.Name; }
		}

		public string Description
		{
			get { return buildingInfo.Description; }
		}

		public virtual ResourceAmount[] ResourcesRequired
		{
			get { return null; }
		}

		protected BuildingInfo buildingInfo;

		public void SetUp(BuildingInfo info)
		{
			buildingInfo = info;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (buildingInfo == null)
			{
				buildingInfo = BuildingManagementUi.instance.GetBuildingInfo(buildingName);
			}
			BuildingManagementUi.instance.OpenBuildingInfoPanel(this);
		}
	}
}
