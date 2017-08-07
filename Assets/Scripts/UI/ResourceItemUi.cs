using Jobs;
using Resources;
using UnityEngine;
using UnityEngine.UI;

namespace UI
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
		
		public void UpdateInfo()
		{
			text.text = resource.Amount.ToString();
		}
	}
}
