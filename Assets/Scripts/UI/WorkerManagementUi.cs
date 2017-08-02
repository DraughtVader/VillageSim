using System.Collections.Generic;
using Jobs;
using UnityEngine;

namespace UI
{
	public class WorkerManagementUi : MonoBehaviour
	{
		[SerializeField]
		protected JobItemUI jobItemPrefab;

		[SerializeField]
		protected Transform content;

		public void SetUp(List<Job> jobs)
		{
			foreach (var job in jobs)
			{
				var item = Instantiate(jobItemPrefab, content);
				item.SetUp(job);
			}
		}
	}
}
