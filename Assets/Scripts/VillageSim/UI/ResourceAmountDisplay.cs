using UnityEngine;
using UnityEngine.UI;

namespace VillageSim.UI
{
	public class ResourceAmountDisplay : MonoBehaviour
	{
		[SerializeField]
		protected Text amountText;

		[SerializeField]
		protected Image imageIcon;

		public void SetUp(string amount, Sprite icon)
		{
			amountText.text = amount;
			imageIcon.sprite = icon;
		}

		public void UpdateText(string amount)
		{
			amountText.text = amount;
		}
	}
}
