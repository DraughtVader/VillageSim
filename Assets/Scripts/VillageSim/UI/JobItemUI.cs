using VillageSim.Jobs;
using UnityEngine;
using UnityEngine.UI;

namespace VillageSim.UI
{
    public class JobItemUI : MonoBehaviour
    {
        [SerializeField]
        protected Text jobTitle,
            jobCount;

        private Job job;

        public void SetUp(Job jobToAssign)
        {
            job = jobToAssign;
            job.SetUp(this);
            UpdateInfo();
        }

        public void UpdateInfo()
        {
            jobTitle.text = string.Format("{0}s", job.JobType);
            jobCount.text = string.Format("{0}/{1}({2})", job.CurrentWorkers, job.DesiredWorkers, job.SupportedWorkers);
        }

        public void PlusButton()
        {
            job.DesiredWorkers++;
        }
    
        public void MinusButton()
        {
            job.DesiredWorkers--;
        }
    }
}
