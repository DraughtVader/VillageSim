using System;
using UI;
using UnityEngine;

namespace Jobs
{
    [Serializable]
    public class Job
    {
        public enum Type
        {
            Lumberjack,
            Forager,
            StoneMiner,
            Idle
        }

        public enum State
        {
            Working,
            Recuperation,
            NoWork
        }

        [SerializeField]
        protected Type type;

        [SerializeField]
        protected Sprite rightArmTool;

        private int currentWorkers;
        private int desiredWorkers;

        public Type JobType
        {
            get { return type; }
        }

        public JobItemUI JobItemUi { get; set; }
        
        public Sprite RightArmTool
        {
            get { return rightArmTool; }
        }

        public int CurrentWorkers
        {
            get { return currentWorkers; }
            set
            {
                if (value < 0)
                {
                    return;
                }
                currentWorkers = value; 
                JobItemUi.UpdateInfo();
            }
        }

        public int DesiredWorkers
        {
            get { return desiredWorkers; }
            set 
            { 
                if (value < 0)
                {
                    return;
                }
                desiredWorkers = value;
                JobItemUi.UpdateInfo();
            }
        }
    }
}