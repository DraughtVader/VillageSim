using UnityEngine;
using VillageSim.Jobs;

namespace Buildings
{
    public class ConstructionDropOff : DropOffLocation
    {
        [SerializeField]
        protected ConstructionSite constructionSite;

        private bool completed;

        public override void DropOff(Collectable collectable)
        {
            Destroy(collectable.gameObject);
            constructionSite.DropOff(collectable.CollectableType, this);
        }

        public void SetUp(Collectable.Type type, ConstructionSite site)
        {
            collectableType = type;
            constructionSite = site;
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