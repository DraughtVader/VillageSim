using VillageSim.Jobs;
using VillageSim.UI;
using UnityEngine;

namespace VillageSim.Resources
{
    [System.Serializable]
    public class Resource
    {
        [SerializeField]
        protected  Collectable.Type type;
        
        [SerializeField]
        protected Sprite icon;

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
                ResourceItemUi.UpdateInfo();
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
        public int Current;
    }
}