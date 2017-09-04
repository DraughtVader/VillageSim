using System;
using Core.Utilities;
using UnityEngine;
using UnityEngine.UI;
using VillageSim.Buildings;
using VillageSim.Jobs;

namespace VillageSim.UI
{
	public class BuildingManagementUi : Singleton<BuildingManagementUi>
	{
		public event Action OpenInfoPanel;
		
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

		public int CurrentHousing { get; private set; }

		protected void Start () 
		{
			foreach (var building in buildingData.BuildingInfo)
			{
				var button = Instantiate(buildingButtonPrefab, content);
				button.SetUp(building, this);
			}
			JobManager.instance.OpenInfoPanel += OnOpenBuildingPanel;
			VillageManager.instance.NewVillager += OnNewVillager;
		}

		public void AdjustHousing(int value)
		{
			CurrentHousing += value;
			UpdateHouseDisplay();
		}

		public void CreateBuildingBlueprint(BuildingInfo buildingInfo)
		{
			placementBlueprint.SetUp(buildingInfo);
			OpenBuildingInfoPanel(buildingInfo);
		}

		public void OpenBuildingInfoPanel(IBuilding building)
		{
			buildingInfoDisplay.OpenPanel(building);
			if (OpenInfoPanel != null)
			{
				OpenInfoPanel();
			}
		}
		
		private void OnNewVillager()
		{
			UpdateHouseDisplay();
		}

		private void UpdateHouseDisplay()
		{
			housingText.text = string.Format("{1}/{0}",  CurrentHousing, VillageManager.instance.VillagerCount);
		}

		private void OnOpenBuildingPanel()
		{
			buildingInfoDisplay.ClosePanel();
		}

		public void UpdateBuildingInfo(IBuilding building)
		{
			buildingInfoDisplay.UpdateBuildingInfo(building);
		}

		public BuildingInfo GetBuildingInfo(string buildingName)
		{
			return buildingData.GetBuildingInfo(buildingName);
		}
	}
}
