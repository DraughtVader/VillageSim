using UnityEngine;

namespace Jobs
{
    public class Collectable : RegisterWorldObject
    {
        [SerializeField]
        protected Type type;
        
        public enum State
        {
            InWorld,
            Targeted,
            Held
        }

        public enum Type
        {
            Wood,
            Food,
            Special,
            None
        }
        
        public DropOffLocation DropOffLocation { get; set; }
        public State CollectableState { get; set; }

        public override Type CollectableType
        {
            get { return type; }
        }

        public override void OnWorkerInteraction(Worker worker)
        {
            base.OnWorkerInteraction(worker);
            CollectableState = State.Held;
            JobManager.instance.Deregister(this, CollectableType);
            worker.PickUpItem(this);
        }

        protected override void Awake()
        {
            base.Awake();
            CollectableState = State.InWorld;
        }

        public override bool IsAvailableToWorker(Worker worker)
        {
            return CollectableState == State.InWorld;
        }
    }
}
