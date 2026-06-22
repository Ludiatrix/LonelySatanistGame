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
                if (milestone.PowerLevel > powerLevel && !milestone.Collected)
                {
                    Debug.Log($"[MilestoneData] Player has reached Milestone {milestone.PowerLevel} and will be awarded {milestone.TapeAmount} Tape!");
                    tapeCollected += milestone.TapeAmount; // Just making sure we snag anyone possibly left behind
                    milestone.Collected = true;
                    return tapeCollected;
                }
            }

            return 0;
        }

        public Milestone GetMilestoneAtPower(int powerLevel)
        {
            foreach (var milestone in Milestones)
            {
                if (milestone.PowerLevel == powerLevel && !milestone.Collected)
                {
                    return  milestone;
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
