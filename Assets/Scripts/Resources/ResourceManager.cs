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

		[SerializeField]
		protected ResourceChangeDisplay resourceChangeDisplayPrefab;

		public void DropOffResource(Collectable.Type type, Vector3 location)
		{
			DropOffResource(GetResource(type), location);
		}
		
		public void DropOffResource(Resource resource, Vector3 location)
		{
			InitResourceChangeDisplay(resource, 1, location);
			resource.Amount++;
		}
		
		public void TakeResource(Collectable.Type type, Vector3 location)
		{
			TakeResource(GetResource(type), location);
		}
		
		public void TakeResource(Resource resource, Vector3 location)
		{
			InitResourceChangeDisplay(resource, -1, location);
			resource.Amount--;
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

		private void InitResourceChangeDisplay(Resource resource, int change, Vector3 position)
		{
			var info = Instantiate(resourceChangeDisplayPrefab, position, Quaternion.identity);
			info.SetUp(resource.Icon, change);
		}
	}
}
