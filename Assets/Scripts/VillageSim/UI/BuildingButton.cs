using UnityEngine;
using UnityEngine.UI;
using VillageSim.Buildings;
using VillageSim.Resources;

namespace VillageSim.UI
{
	[RequireComponent(typeof(Button))]
	public class BuildingButton : MonoBehaviour
	{
		[SerializeField]
		protected Text nameText;

		[SerializeField]
		protected Transform costPanel;

		[SerializeField]
		protected ResourceAmountDisplay resourceAmountDisplayPrefab;

		[SerializeField]
		protected Image icon;
		
		private BuildingInfo buildingInfo;
		private BuildingManagementUi buildingManagementUi;
		
		public void ButtonClick()
		{
			buildingManagementUi.CreateBuildingBlueprint(buildingInfo);
		}

		public void SetUp(BuildingInfo info, BuildingManagementUi manager)
		{
			icon.sprite = info.Icon;
			nameText.text = info.Name;
			buildingInfo = info;
			buildingManagementUi = manager;

			foreach (var resource in buildingInfo.ResourcesRequired)
			{
				var resourceDisplay = Instantiate(resourceAmountDisplayPrefab, costPanel);
				resourceDisplay.SetUp(resource.Requirement.ToString(), ResourceManager.instance.GetResource(resource.Type).Icon);
			}
		}
	}
}
