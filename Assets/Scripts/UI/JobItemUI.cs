using Jobs;
using UnityEngine;
using UnityEngine.UI;

namespace UI
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
            job.JobItemUi = this;
            UpdateInfo();
        }

        public void UpdateInfo()
        {
            jobTitle.text = string.Format("{0}s", job.JobType);
            jobCount.text = string.Format("{0}/{1}", job.CurrentWorkers, job.DesiredWorkers);
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
