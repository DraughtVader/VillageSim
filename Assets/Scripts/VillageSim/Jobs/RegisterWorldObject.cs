namespace VillageSim.Jobs
{
    public abstract class RegisterWorldObject : WorldObject
    {
        protected bool hasRegistered;
        public abstract Collectable.Type CollectableType { get; }

        protected virtual void Start()
        {
            if (!hasRegistered && JobManager.instance)
            {
                Register();
                hasRegistered = true;
            }
        }

        public abstract bool IsAvailableToWorker(Worker worker);

        protected virtual void Register()
        {
            JobManager.instance.Register(this, CollectableType);
        }

        protected virtual void Destroy()
        {
            if (JobManager.instanceExists)
            {
                JobManager.instance.Deregister(this, CollectableType);
            }
            Destroy(gameObject);
        }
    }
}