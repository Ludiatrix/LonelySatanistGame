using UnityEngine;

namespace LSG.ScriptableObjects
{
    /// <summary>
    /// Base class for DemonData
    /// </summary>
    public abstract class DemonData : ScriptableObject
    {
        public string demonName = "Ekzeema";
        public bool canDateDemonMultipleTimes = false;
        [TextArea(5,10)]public string introDialogue;
        public float minimumPowerLevel;
        public float maximumPowerLevel;
        [TextArea(5,10)] public string boon;
        [TextArea(5,10)] public string bane;
        [TextArea(5,10)] public string boonBaneDialogue;
        [TextArea(5,10)] public string dateOutcome;

        public abstract void ApplyEffect();
        
        public virtual bool CanEncounter(int powerLevel)
        {
            return minimumPowerLevel >= powerLevel && powerLevel <= maximumPowerLevel;
        }
    }
}
