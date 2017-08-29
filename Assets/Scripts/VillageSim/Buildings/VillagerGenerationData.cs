using UnityEngine;
using Random = UnityEngine.Random;

namespace VillageSim.Buildings
{
	[CreateAssetMenu(fileName = "VillagerData", menuName = "Data/Villager Data")]
	public class VillagerGenerationData : ScriptableObject
	{
		[SerializeField]
		protected Sprite[] hairDos;

		[SerializeField]
		protected Sprite[] facialHairs;

		[SerializeField]
		protected Color[] hairColours;

		public Sprite GetRandomHairDo()
		{
			return hairDos[Random.Range(0, hairDos.Length)];
		}
		
		public Sprite GetRandomFacialHairDo()
		{
			return facialHairs[Random.Range(0, facialHairs.Length)];
		}
		
		public Color GetRandomHairColour()
		{
			return hairColours[Random.Range(0, hairColours.Length)];
		}
	}
}
