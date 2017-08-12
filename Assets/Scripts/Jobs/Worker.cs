using Pathing;
using Resources;
using UnityEngine;

namespace Jobs
{
    public class Worker : Agent
    {
        [SerializeField]
        protected GameObject needObject,
            needUnavailableSprite;

        [SerializeField]
        protected SpriteRenderer needSprite,
            rightArmAccessory;
        
        public Job.Type JobType { get; set; }
        
        public Job.State JobState { get; protected set; }

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
            item.CollectableState = Collectable.State.Held;
            
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
                    needObject.SetActive(false);
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
            if (food / maxFood < 0.25)
            {
                needObject.SetActive(true);
                needSprite.sprite = ResourceManager.instance.GetResource(Collectable.Type.Food).Icon;
                
                JobState = Job.State.Recuperation;
                var foodSource = JobManager.instance.GetCollectableOrPickUpLocation(this, Collectable.Type.Food);
                if (foodSource != null)
                {
                    MoveTo(foodSource);
                    needUnavailableSprite.SetActive(false);
                    return;
                }
                else
                {
                    needUnavailableSprite.SetActive(true);
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
            if ((JobState == Job.State.NoWork || JobType == Job.Type.Idle) && Input.GetKeyDown(KeyCode.A))
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

        public void AssignJob(Job job)
        {
            JobType = job.JobType;
            rightArmAccessory.sprite = job.RightArmTool;

        }
    }
}