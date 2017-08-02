namespace Jobs
{
    public abstract class RegisterWorldObject : WorldObject
    {
        private bool hasRegistered;
        
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
        protected abstract void Register();
    }
}