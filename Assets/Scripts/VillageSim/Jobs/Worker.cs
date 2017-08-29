using System;
using Pathing;
using VillageSim.Resources;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

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

        [SerializeField]
        protected SpriteRenderer hairSpriteRenderer, 
            facialHairSpriteRenderer, 
            leftBrowSpriteRenderer, 
            rightBrowSpriteRenderer;

        protected float currentJobQueryTime;
        protected bool isDying;
        protected Animator animator;
        protected Job currentJob;
        
        public float Constitution  { get; protected set; }
        public float Skill { get; protected set; }
        public Job.Type JobType { get; set; }
        public Job.State JobState { get; protected set; }
        public Collectable HeldItem { get; set; }

        public float MoveSpeedModifier
        {
            get { return moveSpeedModifier; }
        }

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

        public void SetUpHair(Sprite hair, Sprite facialHair, Color color)
        {
            hairSpriteRenderer.sprite = hair;
            facialHairSpriteRenderer.sprite = facialHair;
            hairSpriteRenderer.color = facialHairSpriteRenderer.color =
                leftBrowSpriteRenderer.color = rightBrowSpriteRenderer.color = color;
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

        public void OnDieAnimationComplete()
        {
            Destroy(gameObject);
        }

        public void SetName(string villagerName)
        {
            Name = villagerName;
        }

        protected void Die()
        {
            isDying = true;
            animator.SetTrigger("Die");
            JobManager.instance.RemoveWorker(JobType);
            GetComponent<Collider2D>().enabled = false;
            DropItem();
            VillageManager.instance.DeregisterVillager(this);
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
            Name = VillageManager.instance.VillagerGenerator.GetRandomName();
            VillageManager.instance.VillagerGenerator.SetUpRandomVillager(this);
            VillageManager.instance.RegisterVillager(this);

            float r0 = Random.Range(-0.25f, 0.25f),
                  r1 = Random.Range(-0.25f, 0.25f),
                  r2 = - (r0 + r1);

            if (Random.value < 0.34)
            {
                float temp = r2;
                r2 = r0;
                r0 = temp;
            }
            else if (Random.value < 0.67)
            {
                float temp = r2;
                r2 = r1;
                r1 = temp;
            }

            moveSpeedModifier = 1 + r0;
            Skill = 1 + r1;
            Constitution = 1 + r2;
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
            energy -= baseEnergyDrain * Time.deltaTime * ( 1 / Constitution);
            food -= baseFoodDrain * Time.deltaTime * (1 / Constitution);
            if (food < 0)
            {
                Die();
            }
        }
    }
}