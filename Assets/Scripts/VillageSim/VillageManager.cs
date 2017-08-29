using System;
using System.Collections.Generic;
using Core.Utilities;
using UnityEngine;
using VillageSim.Jobs;
using VillageSim.UI;

namespace VillageSim
{
	public class VillageManager : Singleton<VillageManager>
	{
		public event Action NewVillager;
		
		[SerializeField]
		protected VillagerGenerator villagerGenerator;

		[SerializeField]
		protected Range newArrivalTimeRange;

		[SerializeField]
		protected Worker villagerPrefab;

		private float currentArrivalCount,
			currentArrivaTime;
		
		protected List<Worker> villagers = new List<Worker>();

		public int VillagerCount
		{
			get { return villagers.Count; }
		}
		
		public VillagerGenerator VillagerGenerator
		{
			get { return villagerGenerator; }
		}

		public void RegisterVillager(Worker villager)
		{
			if (!villagers.Contains(villager))
			{
				villagers.Add(villager);
			}
		}
		
		public void DeregisterVillager(Worker villager)
		{
			if (villagers.Contains(villager))
			{
				villagers.Remove(villager);
			}
		}

		private void CreateNewVillager()
		{
			var newVillager = Instantiate(villagerPrefab, Vector3.zero, Quaternion.identity);
			newVillager.SetName(villagerGenerator.GetRandomName());
			if (NewVillager != null)
			{
				NewVillager();
			}
		}

		private void Start()
		{
			currentArrivaTime = newArrivalTimeRange.GetRandom();
		}

		private void Update()
		{
			if (villagers.Count < BuildingManagementUi.instance.CurrentHousing)
			{
				currentArrivalCount += Time.deltaTime;
				if (currentArrivalCount >= currentArrivaTime)
				{
					currentArrivalCount = 0;
					currentArrivaTime = newArrivalTimeRange.GetRandom();
					CreateNewVillager();
				}
			}
		}
	}
}
	