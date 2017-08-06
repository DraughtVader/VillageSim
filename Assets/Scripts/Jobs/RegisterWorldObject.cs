namespace Jobs
{
    public abstract class RegisterWorldObject : WorldObject
    {
        protected bool hasRegistered;
        public abstract Collectable.Type CollectableType { get; }

        protected override void Awake()
        {
            base.Awake();
            if (JobManager.instanceExists)
            {
                Register();
                hasRegistered = true;
            }
        }

        protected virtual void Start()
        {
            if (!hasRegistered && JobManager.instance)
            {
                Register();
                hasRegistered = true;
            }
        }

        public abstract bool IsAvailableToWorker(Worker worker);

        protected void Register()
        {
            JobManager.instance.Register(this, CollectableType);
        }
    }
}