using UnityEngine;

namespace LSG.ScriptableObjects
{
    [CreateAssetMenu(fileName = "DemonData", menuName = "LSG/Create Demon Data", order = 0)]

    public class DemonData : ScriptableObject
    {
        public string demonName;
        public string concept;
        public float minimumPowerLevel;
        public float maximumPowerLevel;
        public string outcome;
        public string boon;
        public string bane;
        public string dateOutcome;
    }
}
