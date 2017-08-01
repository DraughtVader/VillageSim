namespace Jobs
{
    public class DropOffLocation : WorldObject
    {
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
    }
}