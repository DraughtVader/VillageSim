using Resources;
using UnityEngine;

namespace Jobs
{
    public class PickUpLocation : RegisterWorldObject
    {
        [SerializeField]
        protected Collectable collectablePrefab;

        protected Resource resource;
        
        public void InventoryRemoved(int number)
        {
            resource.Amount -= number;
        }

        public override Collectable.Type CollectableType
        {
            get { return collectablePrefab.CollectableType; }
        }

        public override bool IsAvailableToWorker(Worker worker)
        {
            return resource.Amount > 0;
        }

        public override void OnWorkerInteraction(Worker worker)
        {
            if (resource.Amount > 0)
            {
                var collectable = Instantiate(collectablePrefab, worker.transform);
                ResourceManager.instance.TakeResource(resource, transform.position);
                worker.PickUpItem(collectable);
            }
        }

        protected override void Start()
        {
            base.Start();
            resource = ResourceManager.instance.GetResource(collectablePrefab.CollectableType);
        }
    }
}