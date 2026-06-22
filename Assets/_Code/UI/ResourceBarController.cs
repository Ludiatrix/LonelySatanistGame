using System;
using System.Linq;
using LSG.Classes;
using LSG.Core;
using LSG.Utils;
using TMPro;
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
        [SerializeField] private TMP_Text powerText;
        [SerializeField] private GameObject templateMilestoneMarker;
        [SerializeField] private Transform milestoneContainer;
        [SerializeField] private float maximum = 48f;

        private const string PowerLabel = "Power";

        private void OnEnable()
        {
            // Power only changes when a page is read or a boon/bane payload is applied, and it
            // resets when a new summoning starts. Refresh on those events instead of every frame.
            GameEvents.PageRead?.AddListener(RefreshBar);
            EconomyEvents.SendPayload?.AddListener(OnPayloadApplied);
            PhaseEvents.SummoningPhaseStarted.AddListener(RefreshBar);
        }

        private void OnDisable()
        {
            GameEvents.PageRead?.RemoveListener(RefreshBar);
            EconomyEvents.SendPayload?.RemoveListener(OnPayloadApplied);
            PhaseEvents.SummoningPhaseStarted.RemoveListener(RefreshBar);
        }

        private void Start()
        {
            _economy = DataManager.Instance.PlayerEconomySource;
            powerSlider.maxValue = maximum;
            GenerateMilestones();
            RefreshBar();
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

        private void OnPayloadApplied(ModifierPayload payload) => RefreshBar();

        private void RefreshBar()
        {
            if (_economy == null) _economy = DataManager.Instance.PlayerEconomySource;
            powerSlider.value = _economy.Power;
            SetPowerText(_economy.Power);
        }

        private void SetPowerText(int power)
        {
            if (powerText != null)
            {
                powerText.text = $"{PowerLabel} {power}";
            }
        }
    }
}
