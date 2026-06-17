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
    }
}
