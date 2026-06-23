using System;
using System.Linq;
using LSG.Classes;
using LSG.Core;
using UnityEngine;

namespace LSG.UI
{
    /// <summary>
    /// Controller for reading the MilestoneData asset and checking to see if it needs to update it's own UI.
    /// </summary>
    public class MilestoneMarkerController : MonoBehaviour
    {
        public int MilestonePower = 0;

        [Tooltip("The vertical bar/tick drawn on the slider. Hidden along with the tape icons once the milestone is reached.")]
        [SerializeField] private GameObject markerBar;
        [SerializeField] private GameObject[] tapeIcons;

        private void OnEnable()
        {
            GameEvents.TapeEarnedEvent?.AddListener(OnTapeEarnedEvent);
            PhaseEvents.SummoningPhaseStarted?.AddListener(CheckMilestoneMarkerForTape);
        }

        private void OnDisable()
        {
            GameEvents.TapeEarnedEvent?.RemoveListener(OnTapeEarnedEvent);
            PhaseEvents.SummoningPhaseStarted?.RemoveListener(CheckMilestoneMarkerForTape);
        }

        private void Start()
        {
            CheckMilestoneMarkerForTape();
        }

        private void OnTapeEarnedEvent()
        {
            CheckMilestoneMarkerForTape();
        }

        private void CheckMilestoneMarkerForTape()
        {
            Milestone myMilestone = DataManager.Instance.MilestoneDataSource.GetMilestoneByPowerLevel(MilestonePower);

            // The whole marker (bar + tape icons) is only shown while this milestone is
            // still up for grabs. Once it's been reached (Collected) this game, hide it.
            bool visible = myMilestone != null && !myMilestone.Collected;

            if (markerBar != null) markerBar.SetActive(visible);
            ShowTapeIcon(visible ? myMilestone.TapeAmount : 0);
        }

        /// <summary>Shows the single tape icon for the given amount (0 hides them all).</summary>
        private void ShowTapeIcon(int amount)
        {
            for (int i = 0; i < tapeIcons.Length; i++)
            {
                tapeIcons[i].SetActive(i + 1 == amount);
            }
        }
    }
}
