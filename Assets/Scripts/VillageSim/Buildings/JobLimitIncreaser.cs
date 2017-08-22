using UnityEngine;
using VillageSim.Jobs;

namespace VillageSim.Buildings
{
	public class JobLimitIncreaser : MonoBehaviour
	{
		[SerializeField]
		protected Job.Type job;

		[SerializeField]
		protected int increase;
		
		private void Start () 
		{
			JobManager.instance.IncreaseJobLimit(job, increase);
		}
	}
}
