using LSG.ScriptableObjects;
using UnityEngine;

namespace LSG
{
    /// <summary>
    /// A list of demons you can date! (I can't believe we're making this...)
    /// </summary>
    [CreateAssetMenu(fileName = "DemonDatingPool", menuName = "LSG/Make a Demon Dating Pool")]
    public class DemonDatingPool : ScriptableObject
    {
        [SerializeField] private DemonData[] demons;

        /// <summary>
        /// Summon a demon! It's not very safe.
        /// </summary>
        /// <param name="powerLevel">The general power level of the demon you want to summon</param>
        /// <returns>A demon. I thought that was clear.</returns>
        public DemonData EncounterDemonBasedOnPower(int powerLevel)
        {
            demons.Shuffle(); //SHUFFLE THE DEMONS!
            
            for (int i = 0; i < demons.Length; i++)
            {
                if (demons[i].minimumPowerLevel < powerLevel && demons[i].minimumPowerLevel >= 0)
                {
                    return demons[i];
                }
            }

            return null;
        }
    }
}
