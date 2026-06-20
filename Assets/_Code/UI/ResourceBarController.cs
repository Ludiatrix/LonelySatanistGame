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
        [SerializeField] private float maximum = 48f;

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
            powerSlider.maxValue = maximum;
            UpdateBar();
            GenerateMilestones();
        }

        private void GenerateMilestones()
        {
            foreach (var milestone in DataManager.Instance.MilestoneDataSource.Milestones)
            {
                GameObject go = Instantiate(templateMilestoneMarker, milestoneContainer, false);
                go.SetActive(true);

                Vector3 position = go.transform.localPosition;
                position.x = GetHorizontalOffset(milestone.PowerLevel);
                go.transform.localPosition = position;

                go.GetComponent<MilestoneMarkerController>().MilestonePower = milestone.PowerLevel;
            }
        }

        private float GetHorizontalOffset(int power)
        {
            float normalizedDistance = (power / maximum) - 0.5f; // positions in a rect range from -0.5 to +0.5
            float width = ((RectTransform) milestoneContainer.transform).rect.width;
            return width * normalizedDistance;
        }

        private void UpdateBar()
        {
            powerSlider.value = _economy.Power;
            /*
             * TODO: There is a mask on the Resource Bar as we want to pull along the Resource Bar when Milestones progress.
             * We also need to detect when we have surpassed a milestone so we can smoothly pull it to the left with SmoothMover
             * as well as make the TapeIcons disappear.
             * 
             */
        }
    }
}
