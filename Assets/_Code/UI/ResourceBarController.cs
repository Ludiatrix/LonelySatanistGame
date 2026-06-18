using System;
using System.Linq;
using LSG.Core;
using LSG.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace LSG.UI
{
    /// <summary>
    /// Visual Controller for the PowerBar. Just here to look pretty and does not govern player state.
    /// For that, you want to look at SummoningPhase.cs
    /// </summary>
    public class ResourceBarController : MonoBehaviour
    {
        private PlayerEconomy _economy;

        [SerializeField] private Slider powerSlider;
        [SerializeField] private GameObject templateMilestoneMarker;
        [SerializeField] private Transform milestoneContainer;

        private void OnEnable()
        {
            GameEvents.TapeEarnedEvent?.AddListener(UpdateBar);
        }

        private void OnDisable()
        {
            GameEvents.TapeEarnedEvent?.RemoveListener(UpdateBar);
        }

        private void Start()
        {
            _economy = DataManager.Instance.PlayerEconomySource;
            GenerateMilestones();
        }

        private void GenerateMilestones()
        {
            for (int i = 0; i < DataManager.Instance.MilestoneDataSource.NumberOfMilestonesToGenerate; i++)
            {
                GameObject go = Instantiate(templateMilestoneMarker, milestoneContainer, false);
                go.SetActive(true);
                go.GetComponent<MilestoneMarkerController>().MilestoneMarkerID = (i + 1); // We start at 1
            }
        }

        private void UpdateBar()
        {
            //powerSlider.value = _economy.NormalizedPower;
            //milestoneContainer.GetComponent<SmoothMover>().MoveToTarget(milestoneContainer.localPosition - Vector3.left, 2.0f);
            /*
             * TODO: There is a mask on the Resource Bar as we want to pull along the Resource Bar when Milestones progress.
             * We also need to detect when we have surpassed a milestone so we can smoothly pull it to the left with SmoothMover
             * as well as make the TapeIcons disappear.
             * 
             */
        }
    }
}
