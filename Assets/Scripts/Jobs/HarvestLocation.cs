using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Jobs
{
    public class HarvestLocation : RegisterWorldObject
    {
        private Action harvestComplete;
        
        [SerializeField]
        protected float timeToHarvest = 5.0f;

        [SerializeField]
        protected Collectable collectablesToSpawn;

        [SerializeField]
        protected int numberOfCollectablesToSpawn;

        [SerializeField]
        protected int maxConcurrentWorkers = 2;

        private float currentHarvestTime;
        private int currentNumberWorkers;
        private int workersEnRoute;

        public void AddWorkerEnRouter()
        {
            workersEnRoute++;
        }
        
        public override  Collectable.Type CollectableType
        {
            get { return collectablesToSpawn.CollectableType; }
        }

        public override void OnWorkerInteraction(Worker worker)
        {
            base.OnWorkerInteraction(worker);
            StartHarvest(worker);
        }

        private void StartHarvest(Worker worker)
        {
            if (currentNumberWorkers < maxConcurrentWorkers)
            {
                harvestComplete += worker.OnHarvestComplete;
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
            currentHarvestTime += Time.deltaTime * currentNumberWorkers;
            if (currentHarvestTime >= timeToHarvest)
            {
                for (int i = 0; i < numberOfCollectablesToSpawn; i++)
                {
                    Instantiate(collectablesToSpawn, transform.position + (Vector3)Random.insideUnitCircle, Quaternion.identity);
                }
                currentHarvestTime = 0;

                if (harvestComplete != null)
                {
                    harvestComplete();
                }
                harvestComplete = null;
                currentNumberWorkers = 0;
            }
        }

        public override bool IsAvailableToWorker(Worker worker)
        {
            return (currentNumberWorkers + workersEnRoute) < maxConcurrentWorkers;
        }
    }
}