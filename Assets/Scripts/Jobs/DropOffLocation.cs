using UnityEngine;

namespace Jobs
{
    public class DropOffLocation : RegisterWorldObject
    {
        [SerializeField]
        protected Collectable.Type collectableType;

        public Collectable.Type CollectableType
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
            Destroy(collectable.gameObject);
        }

        protected override void Register()
        {
            JobManager.instance.RegisterDropOffLocation(this);
        }
    }
}