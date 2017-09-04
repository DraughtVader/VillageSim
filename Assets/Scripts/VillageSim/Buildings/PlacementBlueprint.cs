using UnityEngine;
using UnityEngine.EventSystems;
using VillageSim.Pathing;

namespace VillageSim.Buildings
{
    public class PlacementBlueprint : MonoBehaviour
    {
        [SerializeField]
        protected SpriteRenderer spriteRenderer;

        [SerializeField]
        protected ConstructionSite constructionSitePrefab;

        private BuildingInfo buildingInfo;
        private bool validPlacement;
        
        public void SetUp(BuildingInfo building)
        {
            buildingInfo = building;
            gameObject.SetActive(true);
        }
        
        private void Update()
        {
            if (buildingInfo == null)
            {
                return;
            }
            
            var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0;
            position.x = (int)position.x;
            position.y = (int)position.y;
            transform.position = position;
            
            //check for valid placement
            validPlacement = MapManager.instance.IsAreaWalkable((int) position.x, (int) position.y, 2, 2);
            spriteRenderer.color = validPlacement ? Color.green : Color.red;


            bool ui = EventSystem.current.currentSelectedGameObject != null;
            if (Input.GetMouseButton(0) && validPlacement && !ui)
            {
                var constructionSite = Instantiate(constructionSitePrefab, position, Quaternion.identity);
                constructionSite.SetUp(buildingInfo);
                    
                buildingInfo = null;
                gameObject.SetActive(false);
            }
            else if (Input.GetMouseButton(1))
            { 
                //cancel
                buildingInfo = null;
                gameObject.SetActive(false);
            }
        }
    }
}