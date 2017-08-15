using UnityEngine;
using VillageSim.Buildings;

namespace VillageSim.UI
{
	public class BuildingManagementUi : MonoBehaviour
	{
		[SerializeField]
		protected BuildingData buildingData;

		[SerializeField]
		protected Transform content;

		[SerializeField]
		protected BuildingButton buildingButtonPrefab;

		protected void Start () 
		{
			foreach (var building in buildingData.BuildingInfo)
			{
				var button = Instantiate(buildingButtonPrefab, content);
				button.SetUp(building);
			}
		}
	}
}
