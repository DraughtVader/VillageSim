using UnityEngine;

namespace Jobs
{
    public class HarvestLocation : WorldObject
    {
        [SerializeField]
        protected float timeToHarvest = 5.0f;

        [SerializeField]
        protected Collectable collectablesToSpawn;

        [SerializeField]
        protected int numberOfCollectablesToSpawn;

        private float currentHarvestTime = 0.0f;
        protected Worker currentWorker;

        public bool Harvestable { get; protected set; }

        public override void OnWorkerInteraction(Worker worker)
        {
            base.OnWorkerInteraction(worker);
            StartHarvest(worker);
        }

        private void StartHarvest(Worker worker)
        {
            currentWorker = worker;
        }

        private void Update()
        {
            if (currentWorker == null)
            {
                return;
            }
            currentHarvestTime += Time.deltaTime;
            if (currentHarvestTime >= timeToHarvest)
            {
                for (int i = 0; i < numberOfCollectablesToSpawn; i++)
                {
                    Instantiate(collectablesToSpawn, transform.position + (Vector3)Random.insideUnitCircle, Quaternion.identity);
                }
                currentHarvestTime = 0;
                currentWorker.HarvestComplete();
                currentWorker = null;
                Harvestable = false;
            }
        }
    }
}