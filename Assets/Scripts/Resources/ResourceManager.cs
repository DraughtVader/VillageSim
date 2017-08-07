using System.Collections.Generic;
using Core.Utilities;
using Jobs;
using UI;
using UnityEngine;

namespace Resources
{
	public class ResourceManager : Singleton<ResourceManager> 
	{
		[SerializeField]
		protected List<Resource> resources;
		
		[SerializeField]
		protected ResourceManagementUi resourceManagementUi;

		public void DropOffResource(Collectable.Type type)
		{
			GetResource(type).Amount++;
		}
		
		public void TakeResource(Collectable.Type type)
		{
			GetResource(type).Amount--;
		}
		
		public Resource GetResource(Collectable.Type type)
		{
			foreach (var resource in resources)
			{
				if (resource.Type == type)
				{
					return resource;
				}
			}
			return null;
		}
		
		private void Start()
		{
			resourceManagementUi.SetUp(resources);
		}
	}
}
