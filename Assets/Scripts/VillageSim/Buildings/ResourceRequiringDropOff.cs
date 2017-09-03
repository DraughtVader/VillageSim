using UnityEngine;
using VillageSim.Jobs;

namespace VillageSim.Buildings
{
    public class ResourceRequiringDropOff : DropOffLocation
    {
        [SerializeField]
        protected ResourceRequiringTimedObject resourceRequiringObject;

        private bool completed;

        public override void DropOff(Collectable collectable)
        {
            Destroy(collectable.gameObject);
            resourceRequiringObject.DropOff(collectable.CollectableType, this);
        }

        public void SetUp(Collectable.Type type, ResourceRequiringTimedObject site)
        {
            collectableType = type;
            resourceRequiringObject = site;
        }

        public void Complete()
        {
            completed = true;
        }

        public override bool IsAvailableToWorker(Worker worker)
        {
            return base.IsAvailableToWorker(worker) && !completed;
        }
    }
}