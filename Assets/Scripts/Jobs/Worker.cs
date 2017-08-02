using Pathing;
using UnityEngine;

namespace Jobs
{
    public class Worker : Agent
    {
        public Job.Type JobType{ get; set; }

        public Collectable HeldItem { get; set; }

        protected override void OnReachTargetPoint()
        {
            base.OnReachTargetPoint();
            if (targetObject != null)
            {
                targetObject.OnWorkerInteraction(this);
            }
            else
            {
                //idle
            }
        }
        
        public void OnHarvestComplete()
        {
            AskForJob();
        }

        public void DropOffComplete()
        {
            AskForJob();
        }

        protected void AskForJob()
        {
            if (!JobManager.instanceExists)
            {
                return;
            }
            var jobTarget = JobManager.instance.GetJob(this);
            if (jobTarget != null)
            {
                MoveTo(jobTarget);
            }
        }

        protected void Start()
        {
            JobType = Job.Type.Idle;
        }

        protected override void Update()
        {
            base.Update();
            if (Input.GetKeyDown(KeyCode.A))
            {
                AskForJob();
            }
        }
    }
}