using VillageSim.Jobs;
using VillageSim.UI;
using UnityEngine;

namespace VillageSim.Resources
{
    [System.Serializable]
    public class Resource
    {
        [SerializeField]
        protected Collectable.Type type;
        
        [SerializeField]
        protected Sprite icon;

        [SerializeField]
        protected int starting;

        public int Starting
        {
            get { return starting; }
        }
        
        public Collectable.Type Type
        {
            get { return type; }
        }

        public Sprite Icon
        {
            get { return icon; }
        }
        
        public ResourceItemUi ResourceItemUi { get; set; }
        
        protected int amount;

        public virtual bool IsAvailable()
        {
            return amount > 0;
        }

        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                if (ResourceItemUi != null)
                {
                    ResourceItemUi.UpdateInfo();
                }
            }
        }

        public virtual string DisplayAmount()
        {
            return amount.ToString();
        }
    }

    [System.Serializable]
    public class ResourceAmount
    {
        public Collectable.Type Type;
        public int Requirement;
        public int Current { get; set; }

        public int StillRequired
        {
            get { return Requirement - Current; }
        }

        public ResourceAmount(Collectable.Type type, int requirement, int current)
        {
            Type = type;
            Requirement = requirement;
            Current = current;
        }
        
        public ResourceAmount(ResourceAmount toCopy)
        {
            Type = toCopy.Type;
            Requirement = toCopy.Requirement;
        }
    }
}