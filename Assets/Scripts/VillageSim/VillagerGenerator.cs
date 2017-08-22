using UnityEngine;

namespace VillageSim
{
	public class VillagerGenerator : MonoBehaviour
	{
		[SerializeField]
		protected TextAsset namesFile;

		protected string[] names; 

		public string GetRandomName()
		{
			return names[Random.Range(0, names.Length)];
		}

		private void Awake()
		{
			names = namesFile.text.Split('\n');
		}
	}
}
