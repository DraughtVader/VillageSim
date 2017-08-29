using UnityEngine;
using VillageSim.Buildings;
using VillageSim.Jobs;

namespace VillageSim
{
	public class VillagerGenerator : MonoBehaviour
	{
		[SerializeField]
		protected TextAsset namesFile;

		[SerializeField]
		protected VillagerGenerationData data;
		
		[HideInInspector]
		protected string[] names; 
		
		public string GetRandomName()
		{
			return names[Random.Range(0, names.Length)];
		}

		public void SetUpRandomVillager(Worker villager)
		{
			villager.SetUpHair(data.GetRandomHairDo(), data.GetRandomFacialHairDo(), data.GetRandomHairColour());
		}

		private void Awake()
		{
			names = namesFile.text.Split('\n');
		}
	}
}
