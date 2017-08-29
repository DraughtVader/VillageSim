using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Utilities
{
	[Serializable]
	public class Range
	{
		[SerializeField]
		protected float min,
			max;

		public Range(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		public float Min
		{
			get { return min; }
			set { min = value; }
		}

		public float Max
		{
			get { return max; }
			set { max = value; }
		}

		public float GetRandom()
		{
			return Random.Range(min, max);
		}
	}
}
