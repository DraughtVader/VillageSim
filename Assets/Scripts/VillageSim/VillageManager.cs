using Core.Utilities;
using UnityEngine;

namespace VillageSim
{
	public class VillageManager : Singleton<VillageManager>
	{
		[SerializeField]
		protected VillagerGenerator villagerGenerator;
		
		public VillagerGenerator VillagerGenerator
		{
			get { return villagerGenerator; }
		}
	}
}
	