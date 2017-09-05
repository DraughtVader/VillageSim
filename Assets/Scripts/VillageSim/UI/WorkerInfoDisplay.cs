using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VillageSim.Jobs;

namespace VillageSim.UI
{
    public class WorkerInfoDisplay : MonoBehaviour
    {
        [SerializeField]
        protected Text nameText, jobText;

        [SerializeField]
        protected Image foodBar,
            energyBar,
            skillBar,
            speedBar,
            consitutionBar;

        private Canvas canvas;
        private Worker currentWorker;

        public void OpenPanel(Worker worker)
        {
            currentWorker = worker;
            canvas.enabled = true;

            nameText.text = worker.Name;
            jobText.text = worker.JobType.ToString();
        }

        public void ClosePanel()
        {
            canvas.enabled = false;
            currentWorker = null;
        }

        private void Start()
        {
            canvas = GetComponent<Canvas>();
        }

        private void Update()
        {
            if (currentWorker == null)
            {
                return;
            }
            foodBar.fillAmount = currentWorker.NormalizedFood;
            foodBar.color = GetColorBar(currentWorker.NormalizedFood);
            
            energyBar.fillAmount = currentWorker.NormalizedEnergy;
            energyBar.color = GetColorBar(currentWorker.NormalizedEnergy);

            skillBar.fillAmount = currentWorker.Skill * 0.5f;
            speedBar.fillAmount = currentWorker.MoveSpeedModifier * 0.5f;
            consitutionBar.fillAmount = currentWorker.Constitution * 0.5f;
        }

        private Color GetColorBar(float value)
        {
            bool first = value >= 0.5;
            return new Color(first ? (1 - value) * 2f : 1, first ? 1 : value * 2, 0, 1);
        }
    }
}