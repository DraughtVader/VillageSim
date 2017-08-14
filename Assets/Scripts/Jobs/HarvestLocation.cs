using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VillageSim.Jobs
{
    public class HarvestLocation : TimedRegisterObject
    {
        [SerializeField]
        protected Collectable collectablesToSpawn;

        [SerializeField]
        protected int numberOfCollectablesToSpawn;

        public override  Collectable.Type CollectableType
        {
            get { return collectablesToSpawn.CollectableType; }
        }

        protected override void OnWorkComplete()
        {
            for (int i = 0; i < numberOfCollectablesToSpawn; i++)
            {
                Instantiate(collectablesToSpawn, transform.position + (Vector3)Random.insideUnitCircle, Quaternion.identity);
            }
        }

    }
}