using VillageSim.Jobs;
using VillageSim.Resources;
using UnityEngine;
using UnityEngine.UI;

namespace VillageSim.UI
{
	public class ResourceItemUi : MonoBehaviour
	{
		[SerializeField]
		protected Image icon;

		[SerializeField]
		protected Text text;

		protected Resource resource;

		public void SetUp(Resource resourceToAssign)
		{
			resource = resourceToAssign;
			icon.sprite = resource.Icon;
			resourceToAssign.ResourceItemUi = this;
			UpdateInfo();
		}
		
		public virtual void UpdateInfo()
		{
            text.text = resource.DisplayAmount();
        }
	}
}
