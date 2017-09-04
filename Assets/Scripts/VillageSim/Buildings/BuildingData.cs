using System;
using UnityEngine;
using VillageSim.Jobs;
using VillageSim.Resources;

namespace VillageSim.Buildings
{
	[CreateAssetMenu(fileName = "BuildingData", menuName = "Data/BuildingData")]
	public class BuildingData : ScriptableObject
	{
		[SerializeField]
		protected BuildingInfo[] buildingInfos;

		public BuildingInfo[] BuildingInfo
		{
			get { return buildingInfos; }
		}

		public BuildingInfo GetBuildingInfo(string buildingName)
		{
			foreach (var building in buildingInfos)
			{
				if (building.Name == buildingName)
				{
					return building;
				}
			}
			return null;
		}
	}

	[Serializable]
	public class BuildingInfo : IBuilding
	{
		[SerializeField]
		protected string name;

		[SerializeField]
		protected string description;
		
		[SerializeField]
		protected Sprite icon;

		[SerializeField]
		protected ResourceAmount[] resourcesRequired;

		[SerializeField]
		protected WorldObject prefab;

		public string Name
		{
			get { return name; }
		}
		
		public string Description
		{
			get { return description; }
		}

		public Sprite Icon
		{
			get { return icon; }
		}

		public ResourceAmount[] ResourcesRequired
		{
			get { return resourcesRequired; }
		}

		public WorldObject Prefab
		{
			get { return prefab; }
		}
	}
}
