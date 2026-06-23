using System;
using System.Collections.Generic;
using LSG.Classes;
using UnityEngine;

namespace LSG.ScriptableObjects
{
    /// <summary>
    /// A scriptable object that holds an array of static Milestones we use for balancing and updating visuals.
    /// </summary>
    [CreateAssetMenu(fileName = "MilestoneData", menuName = "LSG/Create Milestones")]
    public class MilestoneData : ScriptableObject
    {
        public Milestone[] Milestones;

        public int GetTapeAmountAtPower(int powerLevel)
        {
            int tapeCollected = 0;

            foreach (var milestone in Milestones)
            {
                if (milestone.PowerLevel <= powerLevel && !milestone.Collected)
                {
                    tapeCollected += milestone.TapeAmount;
                    milestone.Collected = true;
                }
            }

            return tapeCollected;
        }

        public Milestone GetMilestoneAtPower(int powerLevel)
        {
            foreach (var milestone in Milestones)
            {
                if (milestone.PowerLevel <= powerLevel && !milestone.Collected)
                {
                    return milestone;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the milestone sitting exactly at this power level, regardless of
        /// whether it's been collected. Used by the per-marker UI so each marker can
        /// track its own milestone (collected or not).
        /// </summary>
        public Milestone GetMilestoneByPowerLevel(int powerLevel)
        {
            foreach (var milestone in Milestones)
            {
                if (milestone.PowerLevel == powerLevel)
                {
                    return milestone;
                }
            }

            return null;
        }
        
        public void Reset()
        {
            foreach (var milestone in Milestones)
            {
                milestone.Collected = false;
            }
        }
    }
}
