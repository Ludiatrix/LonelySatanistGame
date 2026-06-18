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
        public int MilestoneMarkerID = 0;
        
        [SerializeField] private PlayerEconomy playerEconomy;
        [SerializeField] private GameObject[] tapeIcons;

        private void OnEnable()
        {
            GameEvents.TapeEarnedEvent?.AddListener(OnTapeEarnedEvent);
        }

        private void OnDisable()
        {
            GameEvents.TapeEarnedEvent?.RemoveListener(OnTapeEarnedEvent);
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
            Milestone myMilestone = playerEconomy.MilestoneDataSource.GetMilestoneAtPower(MilestoneMarkerID);

            if (myMilestone is null)
            {
                return;
            }
            
            int tapeAmount = 0;
            bool toggle = false;
            
            if (myMilestone.Collected)
            {
                tapeAmount = 4;
                
                // Remove the listener at this point
                GameEvents.TapeEarnedEvent?.RemoveListener(OnTapeEarnedEvent);
            }
            else
            {
                if (myMilestone.TapeAmount > 0)
                {
                    tapeAmount = myMilestone.TapeAmount;
                    toggle = true;
                }
            }
            
            ToggleIcons(tapeAmount, toggle);
        }

        private void ToggleIcons(int amount, bool toggle)
        {
            for (int i = 0; i < amount; i++)
            {
                tapeIcons[i].SetActive(toggle);
            }
        }
    }
}
