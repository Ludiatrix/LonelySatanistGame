using System;
using System.Collections.Generic;
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
        public DemonData LastEncounteredDemon => _currentDemon;
        private DemonData _currentDemon = null;
        
        [SerializeField] private DemonData[] demons;

        public DemonData[] Demons => (DemonData[]) demons.Clone();

        public List<DemonData> AvailableDemons = new List<DemonData>();


        /// <summary>
        /// Summon a demon! It's not very safe.
        /// </summary>
        /// <param name="powerLevel">The general power level of the demon you want to summon</param>
        /// <returns>A demon. I thought that was clear.</returns>
        public DemonData EncounterDemonBasedOnPower(int powerLevel)
        {
            AvailableDemons.Shuffle();

            for (int i = 0; i < AvailableDemons.Count; i++)
            {
                DemonData demon = AvailableDemons[i];

                if (demon.minimumPowerLevel <= powerLevel && demon.minimumPowerLevel >= 0)
                {
                    _currentDemon = demon;

                    if (!demon.canDateDemonMultipleTimes)
                    {
                        AvailableDemons.RemoveAt(i);
                        Debug.Log($"Found the Demon {demon}");
                    }

                    return demon;
                }
            }

            return null;
        }

        public void Reset()
        {
            _currentDemon = null;
            AvailableDemons.Clear();
            AvailableDemons.AddRange(Demons);
        }
    }
}
