using System.Collections.Generic;
using UnityEngine;
using VillageSim.Jobs;

namespace VillageSim.Buildings
{
	public class EnergyRecuperater : RegisterWorldObject
	{
		[SerializeField]
		protected int maxOccupants = 3;

		[SerializeField]
		protected float energyRecupertaionPerSecond = 10.0f;

		private int occupantsEnRoute;

		private readonly List<Worker> currentOccupants = new List<Worker>();

		public override Collectable.Type CollectableType
		{
			get { return Collectable.Type.None; }
		}

		public override bool IsAvailableToWorker(Worker worker)
		{
			return currentOccupants.Count + occupantsEnRoute < maxOccupants;
		}
		
		public void AddWorkerEnRouter()
		{
			occupantsEnRoute++;
		}

		public override void OnWorkerInteraction(Worker worker)
		{
			base.OnWorkerInteraction(worker);
			currentOccupants.Add(worker);
			worker.gameObject.SetActive(false);
			occupantsEnRoute--;
			worker.StartedEnergyRecuperation();
		}

		private void Update()
		{
			if (currentOccupants.Count > 0)
			{
				for (var i = currentOccupants.Count - 1; i >= 0; i--)
				{
					var worker = currentOccupants[i];
					bool fullyRecuperated = worker.RecuperateEnergy(energyRecupertaionPerSecond * Time.deltaTime);
					if (fullyRecuperated)
					{
						currentOccupants.RemoveAt(i);
						worker.gameObject.SetActive(true);
						worker.EnergyRecuperationComplete();
					}
				}
			}
		}
	}
}
