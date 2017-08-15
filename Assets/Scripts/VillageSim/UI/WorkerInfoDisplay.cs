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
        protected Image foodBar;

        private Canvas canvas;
        private Worker currentWorker;

        public void OpenPanel(Worker worker)
        {
            currentWorker = worker;
            canvas.enabled = true;

            nameText.text = worker.Name;
            jobText.text = worker.JobType.ToString();
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
        }

        private Color GetColorBar(float value)
        {
            bool first = value >= 0.5;
            return new Color(first ? (1 - value) * 2f : 1, first ? 1 : value * 2, 0, 1);
        }
    }
}