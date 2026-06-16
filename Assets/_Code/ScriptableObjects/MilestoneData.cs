using LSG.TapeSystem;
using UnityEngine;

namespace LSG.ScriptableObjects
{
    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(fileName = "MilestoneData", menuName = "LSG/Create Milestones")]
    public class MilestoneData : ScriptableObject
    {
        public Milestone[] Milestones;
    }
}
