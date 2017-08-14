using System.Collections.Generic;
using VillageSim.Jobs;
using VillageSim.Resources;
using UnityEngine;

namespace VillageSim.UI
{
    public class ResourceManagementUi : MonoBehaviour
    {
        [SerializeField]
        protected ResourceItemUi resourceItemPrefab;

        [SerializeField]
        protected Transform content;

        public void SetUp(List<Resource> Resources)
        {
            foreach (var resource in Resources)
            {
                var item = Instantiate(resourceItemPrefab, content);
                item.SetUp(resource);
            }
        }
    }
}