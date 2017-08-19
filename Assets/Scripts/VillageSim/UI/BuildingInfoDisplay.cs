using UnityEngine;
using UnityEngine.UI;
using VillageSim.Buildings;
using VillageSim.Resources;

namespace VillageSim.UI
{
    public class BuildingInfoDisplay : MonoBehaviour
    {
        [SerializeField]
        protected Text nameText, descriptionText;

        [SerializeField]
        protected Transform resourcePanel;
        
        [SerializeField]
        protected ResourceAmountDisplay resourceAmountDisplayPrefab;

        private Canvas canvas;
        private IBuilding currentBuildingInfo;

        public void OpenPanel(IBuilding building)
        {
            SetPanelOpen(true);
            currentBuildingInfo = building;
            nameText.text = building.Name;
            descriptionText.text = building.Description;
            //clear current resource display
            for (int i = 0; i < resourcePanel.childCount; i++)
            {
                Destroy(resourcePanel.GetChild(i).gameObject);
            }
            foreach (ResourceAmount resource in currentBuildingInfo.ResourcesRequired)
            {
                var resourceDisplay = Instantiate(resourceAmountDisplayPrefab, resourcePanel);
                resourceDisplay.SetUp(resource.StillRequired.ToString(), ResourceManager.instance.GetResource(resource.Type).Icon);
            }
        }

        public void SetPanelOpen(bool open)
        {
            canvas.enabled = open;
        }

        private void Start()
        {
            canvas = GetComponent<Canvas>();
            SetPanelOpen(false);
        }
    }
}