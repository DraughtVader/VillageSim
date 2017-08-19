using Core.Utilities;
using UnityEngine;
using VillageSim.Buildings;

namespace VillageSim.UI
{
	public class BuildingManagementUi : Singleton<BuildingManagementUi>
	{
		[SerializeField]
		protected BuildingData buildingData;

		[SerializeField]
		protected Transform content;

		[SerializeField]
		protected BuildingButton buildingButtonPrefab;
		
		[SerializeField]
		private PlacementBlueprint placementBlueprint;

		[SerializeField]
		protected BuildingInfoDisplay buildingInfoDisplay;

		protected void Start () 
		{
			foreach (var building in buildingData.BuildingInfo)
			{
				var button = Instantiate(buildingButtonPrefab, content);
				button.SetUp(building, this);
			}
		}

		public void CreateBuildingBlueprint(BuildingInfo buildingInfo)
		{
			placementBlueprint.SetUp(buildingInfo);
		}

		public void OpenBuildingInfoPanel(IBuilding building)
		{
			buildingInfoDisplay.OpenPanel(building);
		}
	}
}
