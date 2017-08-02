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

        public Type CollectableType
        {
            get { return type; }
        }

        public override void OnWorkerInteraction(Worker worker)
        {
            base.OnWorkerInteraction(worker);
            transform.SetParent(worker.transform);
            worker.HeldItem = this;
            CollectableState = State.Held;
            JobManager.instance.DeregisterCollectable(this);
            DropOffLocation = JobManager.instance.GetDropOffLocation(worker);
            worker.MoveTo(DropOffLocation);
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

        protected override void Register()
        {
            JobManager.instance.RegisterCollectable(this);
        }
    }
}
