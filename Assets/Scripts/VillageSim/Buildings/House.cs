using System;
using VillageSim.Jobs;

namespace Buildings
{
    public class House : RegisterWorldObject
    {
        public override Collectable.Type CollectableType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool IsAvailableToWorker(Worker worker)
        {
            throw new NotImplementedException();
        }
    }
}
