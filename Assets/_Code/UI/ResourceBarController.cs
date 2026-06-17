using System;
using LSG.Core;
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
        public PlayerEconomy Economy;

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
            GenerateMilestones();
        }

        private void GenerateMilestones()
        {
            /*
             * TODO: See this? This is bad. Bad!
             * However I have a lot to do and this only runs once.
             * We should fix it later, but probably won't.
             * Too Bad!
             */
            foreach (var milestoneToGenerate in Economy.MilestoneDataSource.Milestones)
            {
                GameObject go = Instantiate(templateMilestoneMarker, milestoneContainer, false);
                go.SetActive(true);
                var iconContainer = go.transform.Find("TapeIconContainer").gameObject;
                go.name = $"Milestone {milestoneToGenerate.PowerLevel}";
                for (int i = 0; i < milestoneToGenerate.TapeAmount; i++)
                {
                    iconContainer.transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }

        private void UpdateBar()
        {
            powerSlider.value = Economy.NormalizedPower;
            
            /*
             * TODO: There is a mask on the Resource Bar as we want to pull along the Resource Bar when Milestones progress.
             * We also need to detect when we have surpassed a milestone so we can smoothly pull it to the left with SmoothMover
             * as well as make the TapeIcons disappear.
             * 
             */
        }
    }
}
