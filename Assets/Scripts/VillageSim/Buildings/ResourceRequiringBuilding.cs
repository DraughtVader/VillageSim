using VillageSim.Resources;
using VillageSim.UI;

namespace VillageSim.Buildings
{
	public class ResourceRequiringBuilding : Building
	{
		protected ResourceRequiringTimedObject resourceRequiringBuilding;
		
		public override ResourceAmount[] ResourcesRequired
		{
			get
			{
				return resourceRequiringBuilding.ResourcesRequired;
			}
		}

		private void Start()
		{
			resourceRequiringBuilding = GetComponent<ResourceRequiringTimedObject>();
			resourceRequiringBuilding.DroppedOff += UpdateBuildingInfo;
			resourceRequiringBuilding.TimeComplete += UpdateBuildingInfo;
		}

		private void UpdateBuildingInfo()
		{
			BuildingManagementUi.instance.UpdateBuildingInfo(this);
		}
	}
}
