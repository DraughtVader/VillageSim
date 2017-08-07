using Jobs;
using UI;
using UnityEngine;

namespace Resources
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
        
        private int amount;

        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value; 
                ResourceItemUi.UpdateInfo();
            }
        }
    }
}