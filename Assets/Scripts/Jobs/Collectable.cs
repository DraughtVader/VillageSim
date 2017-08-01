using UnityEngine;

namespace Jobs
{
    public class Collectable : WorldObject
    {
        [SerializeField]
        protected Type type;

        private bool hasRegistered;
        
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
            Special
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
            DropOffLocation = JobManager.instance.GetDropOffLocation(this);
            worker.MoveTo(DropOffLocation);
        }

        protected override void Awake()
        {
            base.Awake();
            CollectableState = State.InWorld;
            if (JobManager.instanceExists)
            {
                JobManager.instance.RegisterCollectable(this);
                hasRegistered = true;
            }
        }

        protected virtual void Start()
        {
            if (!hasRegistered && JobManager.instanceExists)
            {
                JobManager.instance.RegisterCollectable(this);
                hasRegistered = true;
            }
        }
    }
}
