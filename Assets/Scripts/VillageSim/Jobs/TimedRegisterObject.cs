using System;
using UnityEngine;

namespace VillageSim.Jobs
{
	public abstract class TimedRegisterObject : RegisterWorldObject
	{
		public event Action WorkComplete;
		
		[SerializeField]
		protected float timeToWork = 5.0f;

		[SerializeField]
		protected int maxConcurrentWorkers;
		
		private float currentTime;
		private int currentNumberWorkers;
		private int workersEnRoute;
		
		public void AddWorkerEnRouter()
		{
			workersEnRoute++;
		}
		
		public override void OnWorkerInteraction(Worker worker)
		{
			base.OnWorkerInteraction(worker);
			StartWork(worker);
			worker.StartJob();
		}

		private void StartWork(Worker worker)
		{
			if (maxConcurrentWorkers == -1 || currentNumberWorkers < maxConcurrentWorkers)
			{
				WorkComplete += worker.OnJobComplete;
				currentNumberWorkers++;
				workersEnRoute--;
			}
			else
			{
				//TODO
			}
		}
		
		private void Update()
		{
			if (currentNumberWorkers == 0)
			{
				return;
			}
			currentTime += Time.deltaTime * currentNumberWorkers;
			if (currentTime >= timeToWork)
			{
				currentNumberWorkers = 0;
				currentTime = 0;
				OnWorkComplete();

				if (WorkComplete != null)
				{
					WorkComplete();
				}
				WorkComplete = null;
			}
		}

		protected abstract void OnWorkComplete();

		public override bool IsAvailableToWorker(Worker worker)
		{
			return maxConcurrentWorkers == -1 || (currentNumberWorkers + workersEnRoute) < maxConcurrentWorkers;
		}
	}
}
