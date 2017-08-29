using System.Collections.Generic;
using VillageSim.Jobs;
using UnityEngine;

namespace VillageSim.UI
{
	public class WorkerManagementUi : MonoBehaviour
	{
		[SerializeField]
		protected JobItemUI jobItemPrefab;

		[SerializeField]
		protected Transform content;

        [SerializeField]
        protected WorkerInfoDisplay workerInfoDisplay;

		public void SetUp(List<Job> jobs)
		{
			foreach (var job in jobs)
			{
				var item = Instantiate(jobItemPrefab, content);
				item.transform.localScale = Vector3.one;
				item.SetUp(job);
			}
		}

        public void OpenWorkerInfo(Worker worker)
        {
            workerInfoDisplay.OpenPanel(worker);
        }
		
		public void CloseWorkerInfo()
		{
			workerInfoDisplay.ClosePanel();
		}
	}
}
