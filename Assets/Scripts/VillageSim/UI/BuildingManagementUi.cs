using Core.Utilities;
using UnityEngine;
using UnityEngine.UI;
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

		[SerializeField]
		protected Text housingText;

		private int currentHousing;

		protected void Start () 
		{
			foreach (var building in buildingData.BuildingInfo)
			{
				var button = Instantiate(buildingButtonPrefab, content);
				button.SetUp(building, this);
			}
		}

		public void AdjustHousing(int value)
		{
			currentHousing += value;
			housingText.text = currentHousing.ToString();
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
