using System.Collections.Generic;
using Jobs;
using Resources;
using UnityEngine;

namespace UI
{
    public class ResourceManagementUi : MonoBehaviour
    {
        [SerializeField]
        protected ResourceItemUi resourceItemPrefab;

        [SerializeField]
        protected Transform content;

        public void SetUp(List<Resource> resources)
        {
            foreach (var resource in resources)
            {
                var item = Instantiate(resourceItemPrefab, content);
                item.SetUp(resource);
            }
        }
    }
}