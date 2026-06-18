using UnityEngine;

namespace LSG.ScriptableObjects
{
    [CreateAssetMenu(fileName = "DemonData", menuName = "LSG/Create Demon Data", order = 0)]

    public class DemonData : ScriptableObject
    {
        public string demonName;
        [TextArea(5,10)]public string concept;
        public float minimumPowerLevel;
        public float maximumPowerLevel;
        [TextArea(5,10)] public string outcome;
        [TextArea(5,10)] public string boon;
        [TextArea(5,10)] public string bane;
        [TextArea(5,10)] public string boonBaneDialogue;
        [TextArea(5,10)] public string dateOutcome;
    }
}
