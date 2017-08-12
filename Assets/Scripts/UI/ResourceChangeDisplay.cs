using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class ResourceChangeDisplay : MonoBehaviour
	{
		[SerializeField]
		protected Image resourceIcon;

		[SerializeField]
		protected GameObject plus,
			minus;

		public void SetUp(Sprite resource, int change)
		{
			resourceIcon.sprite = resource;
			plus.SetActive(change > 0);
			minus.SetActive(change < 0);
		}

		public void OnFafeAnimationComplete()
		{
			Destroy(gameObject);
		}
	}
}
