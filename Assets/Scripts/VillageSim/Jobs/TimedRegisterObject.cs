using System;
using System.Collections.Generic;
using System.Linq;
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

		private int currentNumberWorkers
		{
			get { return currentWorkers.Count; }
		}
		private int workersEnRoute;

		private readonly List<float> currentWorkers = new List<float>();
		
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
				currentWorkers.Add(worker.Skill);
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
			currentTime += Time.deltaTime * currentWorkers.Sum();
			if (currentTime >= timeToWork)
			{
				currentWorkers.Clear();
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
