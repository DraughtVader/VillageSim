using Pathing;
using VillageSim.Resources;
using UnityEngine;
using UnityEngine.EventSystems;

namespace VillageSim.Jobs
{
    [RequireComponent(typeof(Animator))]
    public class Worker : Agent, IPointerClickHandler
    {
        [SerializeField]
        protected GameObject needObject,
            needUnavailableSprite;

        [SerializeField]
        protected SpriteRenderer needSprite,
            rightArmAccessory;

        [SerializeField]
        protected float jobQueryTime = 1.0f;
        
        [SerializeField]
        protected float maxEnergy = 100,
            maxFood = 100;
        
        protected float energy = 100,
            food = 100;

        [SerializeField]
        protected float baseEnergyDrain = 1,
            baseFoodDrain = 2;

        protected float currentJobQueryTime;

        protected bool isDying;

        protected Animator animator;

        protected Job currentJob;

        public Job.Type JobType { get; set; }
        
        public Job.State JobState { get; protected set; }

        public Collectable HeldItem { get; set; }

        public float NormalizedFood
        {
            get { return food / maxFood; }
        }

        public string Name { get; protected set; }

        protected override void OnReachTargetPoint()
        {
            animator.SetTrigger("Idle");
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
        
        public void OnJobComplete()
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
                    if (item.DropOffLocation == null)
                    {
                        item.transform.parent = null;
                        HeldItem = null;
                        item.CollectableState = Collectable.State.InWorld;
                        AskForJob();
                        return;
                    }
                    MoveTo(item.DropOffLocation);
                    break;
                case Job.State.Recuperation:
                case Job.State.NoWork:
                    // consume for recuperatin
                    Destroy(item.gameObject);
                    food += 50; //TODO fix this garbage
                    needObject.SetActive(false);
                    AskForJob();
                    break;
            }
            
            item.transform.SetParent(transform);
        }
        

        protected void AskForJob()
        {
            if (isDying)
            {
                return;
            }
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

        public override void MoveTo(WorldObject target)
        {
            if (target == null)
            {
                AskForJob();
                return;
            }
            base.MoveTo(target);
            animator.SetTrigger("Walk");
        }

        public void OnDieAnimationComplete()
        {
            Destroy(gameObject);
        }

        protected void Die()
        {
            isDying = true;
            animator.SetTrigger("Die");
            JobManager.instance.RemoveWorker(JobType);
            GetComponent<Collider2D>().enabled = false;
            DropItem();
        }

        protected void DropItem()
        {
            if (HeldItem != null)
            {
                HeldItem.transform.parent = null;
                HeldItem.CollectableState = Collectable.State.InWorld;
                HeldItem.DropOffLocation = null;
                HeldItem = null;
            }
        }
        
        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
        }

        protected void Start()
        {
            JobType = Job.Type.Idle;
            Name = JobManager.instance.VillagerGenerator.GetRandomName();
        }

        protected override void Update()
        {
            if (isDying)
            {
                return;
            }
            base.Update();
            if ((JobState == Job.State.NoWork || JobType == Job.Type.Idle))
            {
                currentJobQueryTime += Time.deltaTime;
                if (currentJobQueryTime >= jobQueryTime)
                {
                    currentJobQueryTime = 0.0f;
                    AskForJob();
                }
            }
            StatAdjustement();
        }

        protected void StatAdjustement()
        {
            energy -= baseEnergyDrain * Time.deltaTime;
            food -= baseFoodDrain * Time.deltaTime;
            if (food < 0)
            {
                Die();
            }
        }

        public void AssignJob(Job job)
        {
            JobType = job.JobType;
            currentJob = job;
            rightArmAccessory.sprite = job.RightArmTool;
        }

        public void StartJob()
        {
            animator.SetTrigger(currentJob.AnimationTrigger);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            JobManager.instance.OpenWorkerInfo(this);
        }
    }
}