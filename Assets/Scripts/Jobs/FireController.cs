using Resources;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Jobs
{
	public class FireController : MonoBehaviour, IPointerClickHandler
	{
		[SerializeField]
		protected ParticleSystem fire,
			burstfx;

		[SerializeField]
		protected float fireDrain = 1.0f,
			healthyFireEmissionRate = 50.0f,
			autoFeedPoint = 0.25f;

		[SerializeField]
		protected int woodHealthIncrease = 50;
		
		protected float currentHealth = 100.0f;
		
		public void OnPointerClick(PointerEventData eventData)
		{
			TryAddWood();
		}

		private void Update()
		{
			currentHealth -= fireDrain * Time.deltaTime;
			float health = currentHealth / 100;
			var emission = fire.emission;

			if (health <= autoFeedPoint)
			{
				if(!TryAddWood())
				{
					//TODO display lack off wood
				}
			}

			emission.rateOverTime = health * healthyFireEmissionRate;
		}

		private bool TryAddWood()
		{
			Resource wood = ResourceManager.instance.GetResource(Collectable.Type.Wood);
			if (wood.Amount <= 0)
			{
				return false;
			}
			currentHealth += woodHealthIncrease;
			ResourceManager.instance.TakeResource(wood, transform.position + new Vector3(0, 1));
			burstfx.Play();
			return true;
		}
	}
}
