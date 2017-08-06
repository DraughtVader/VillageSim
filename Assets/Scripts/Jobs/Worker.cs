using System;
using Pathing;
using UnityEngine;

namespace Jobs
{
    public class Worker : Agent
    {
        public Job.Type JobType { get; set; }
        
        public Job.State JobState { get; set; }

        public Collectable HeldItem { get; set; }

        protected float maxEnergy = 100,
            maxFood = 100;
        
        protected float energy = 100,
            food = 100;

        protected float baseEnergyDrain = 1,
            baseFoodDrain = 2;

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

        public void PickUpItem(Collectable item)
        {
            HeldItem = item;

            switch (JobState)
            {
                case Job.State.Working:
                    // drop off item
                    item.DropOffLocation = JobManager.instance.GetDropOffLocation(this, item.CollectableType);
                    MoveTo(item.DropOffLocation);
                    break;
                case Job.State.Recuperation:
                case Job.State.NoWork:
                    // consume for recuperatin
                    Destroy(item.gameObject);
                    food += 100;
                    AskForJob();
                    break;
            }
            
            item.transform.SetParent(transform);
        }
        

        protected void AskForJob()
        {
            if (!JobManager.instanceExists)
            {
                return;
            }
            
            //Check stats in case recuperation in required
            if (food / maxFood < 0.1)
            {
                JobState = Job.State.Recuperation;
                var foodSource = JobManager.instance.GetCollectableOrPickUpLocation(this, Collectable.Type.Food);
                if (foodSource != null)
                {
                    MoveTo(foodSource);
                    return;
                }
                else
                {
                    //TODO handle this, let player know of shorage
                }
            }
            
            var jobTarget = JobManager.instance.GetJob(this);
            if (jobTarget != null)
            {
                MoveTo(jobTarget);
                JobState = Job.State.Working;
            }
            else
            {
                JobState = Job.State.NoWork;
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
            StatAdjustement();
        }

        protected void StatAdjustement()
        {
            energy -= baseEnergyDrain * Time.deltaTime;
            food -= baseFoodDrain * Time.deltaTime;
        }
    }
}