using Resources;
using UnityEngine;

namespace Jobs
{
    public class DropOffLocation : RegisterWorldObject
    {
        [SerializeField]
        protected Collectable.Type collectableType;

        [SerializeField]
        protected PickUpLocation pickUpLocation;
        
        protected Resource resource;

        public override Collectable.Type CollectableType
        {
            get { return collectableType; }
        }
        
        public override void OnWorkerInteraction(Worker worker)
        {
            base.OnWorkerInteraction(worker);
            DropOff(worker.HeldItem);
            worker.HeldItem = null;
            worker.DropOffComplete();
        }
        
        public virtual void DropOff(Collectable collectable)
        {
            ResourceManager.instance.DropOffResource(resource, transform.position);
            Destroy(collectable.gameObject);
        }


        public override bool IsAvailableToWorker(Worker worker)
        {
            return CollectableType == worker.HeldItem.CollectableType;
        }
        
        protected override void Start()
        {
            base.Start();
            resource = ResourceManager.instance.GetResource(collectableType);
        }
    }
}