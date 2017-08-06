using UnityEngine;

namespace Jobs
{
    public class PickUpLocation : RegisterWorldObject
    {
        [SerializeField]
        protected Collectable collectablePrefab;

        protected int currnetInventory = 0;

        public void InventoryAdded(int number)
        {
            currnetInventory += number;
        }
        
        public void InventoryRemoved(int number)
        {
            currnetInventory -= number;
        }

        public override Collectable.Type CollectableType
        {
            get { return collectablePrefab.CollectableType; }
        }

        public override bool IsAvailableToWorker(Worker worker)
        {
            return currnetInventory > 0;
        }

        public override void OnWorkerInteraction(Worker worker)
        {
            if (currnetInventory > 0)
            {
                var collectable = Instantiate(collectablePrefab, worker.transform);
                currnetInventory--;
                worker.PickUpItem(collectable);
            }
        }
    }
}