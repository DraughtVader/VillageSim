using System.Collections.Generic;
using Core.Utilities;
using VillageSim.Jobs;
using VillageSim.UI;
using UnityEngine;

namespace VillageSim.Resources
{
	public class ResourceManager : Singleton<ResourceManager> 
	{
		[SerializeField]
		protected List<Resource> Resources;
		
		[SerializeField]
		protected ResourceManagementUi resourceManagementUi;

		[SerializeField]
		protected ResourceChangeDisplay resourceChangeDisplayPrefab;

        protected IntVector2 housing;

		public void DropOffResource(Collectable.Type type, Vector3 location, int number = 1)
		{
			DropOffResource(GetResource(type), location, number);
		}
		
		public void DropOffResource(Resource resource, Vector3 location, int number = 1)
		{
			InitResourceChangeDisplay(resource, number, location);
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
			foreach (var resource in Resources)
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
			resourceManagementUi.SetUp(Resources);
		}

		private void InitResourceChangeDisplay(Resource resource, int change, Vector3 position)
		{
			var info = Instantiate(resourceChangeDisplayPrefab, position, Quaternion.identity);
			info.SetUp(resource.Icon, change);
		}
	}
}
