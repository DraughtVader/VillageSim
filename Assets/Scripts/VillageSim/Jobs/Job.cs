using System;
using VillageSim.UI;
using UnityEngine;

namespace VillageSim.Jobs
{
    [Serializable]
    public class Job
    {
        public enum Type
        {
            Lumberjack,
            Forager,
            StoneMiner,
            Builder,
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

        [SerializeField]
        protected string animationTrigger;

        private int currentWorkers;
        private int desiredWorkers;
        private int supportedWorkers;

        public Type JobType
        {
            get { return type; }
        }

        public JobItemUI JobItemUi { get; protected set; }
        
        public Sprite RightArmTool
        {
            get { return rightArmTool; }
        }

        public string AnimationTrigger
        {
            get { return animationTrigger; }
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
                if (JobItemUi != null)
                {
                    JobItemUi.UpdateInfo();
                }
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
                if (JobItemUi != null)
                {
                    JobItemUi.UpdateInfo();
                }
            }
        }
        
        public int SupportedWorkers
        {
            get { return supportedWorkers; }
            set 
            { 
                if (value < 0)
                {
                    return;
                }
                supportedWorkers = value;
                if (JobItemUi != null)
                {
                    JobItemUi.UpdateInfo();
                }
            }
        }

        public void SetUp(JobItemUI ui)
        {
            JobItemUi = ui;
            JobItemUi.UpdateInfo();
        }
    }
}