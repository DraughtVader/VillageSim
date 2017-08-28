using UnityEngine;
using VillageSim.UI;

namespace VillageSim.Buildings
{
	public class HousingLimitIncreaser : MonoBehaviour 
	{
		[SerializeField]
		protected int increase;

		private void Start()
		{
			BuildingManagementUi.instance.AdjustHousing(increase);
		}
	}
}
