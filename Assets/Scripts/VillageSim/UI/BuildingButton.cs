using UnityEngine;
using UnityEngine.UI;
using VillageSim.Buildings;

namespace VillageSim.UI
{
	[RequireComponent(typeof(Button))]
	public class BuildingButton : MonoBehaviour
	{
		[SerializeField]
		protected Text nameText,
			costText;

		[SerializeField]
		protected Image icon;
		
		private BuildingInfo buildingInfo;

		public void ButtonClick()
		{
			
		}

		public void SetUp(BuildingInfo info)
		{
			icon.sprite = info.Icon;
			nameText.text = info.Name;
			buildingInfo = info;
		}
	}
}
