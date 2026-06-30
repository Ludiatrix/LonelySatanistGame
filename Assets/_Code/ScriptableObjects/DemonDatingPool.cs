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

        [Header("Forced encounter demons (not part of the random pool)")]
        [SerializeField] private DemonData papiyawn;
        [SerializeField] private DemonData exhaustedSummons;
        [SerializeField] private DemonData theBook;

        // When set, the next encounter will be this demon regardless of power.
        private DemonData _forcedDemon = null;

        public DemonData[] Demons => (DemonData[]) demons.Clone();
        public DemonData Papiyawn => papiyawn;
        public DemonData ExhaustedSummons => exhaustedSummons;
        public DemonData TheBook => theBook;

        public List<DemonData> AvailableDemons = new List<DemonData>();

        /// <summary>
        /// Forces the next encounter to be the given demon (e.g. a lose-condition
        /// demon). Cleared once it has been handed out.
        /// </summary>
        public void QueueForcedEncounter(DemonData demon) => _forcedDemon = demon;

        /// <summary>True if the next encounter is already locked to a forced demon.</summary>
        public bool HasForcedEncounter => _forcedDemon != null;

        /// <summary>
        /// True if the demon is one of the forced lose-condition encounters. These
        /// should not tick the normal per-encounter stat changes.
        /// </summary>
        public bool IsForcedEncounterDemon(DemonData demon) =>
            demon != null && (demon == papiyawn || demon == theBook || demon == exhaustedSummons);


        /// <summary>
        /// Summon a demon! It's not very safe.
        /// </summary>
        /// <param name="powerLevel">The general power level of the demon you want to summon</param>
        /// <returns>A demon. I thought that was clear.</returns>
        public DemonData EncounterDemonBasedOnPower(int powerLevel)
        {
            // A forced encounter (e.g. a lose condition) overrides power-based selection.
            if (_forcedDemon != null)
            {
                _currentDemon = _forcedDemon;
                _forcedDemon = null;
                return _currentDemon;
            }

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

            return ExhaustedSummons;
        }

        public void Reset()
        {
            _currentDemon = null;
            _forcedDemon = null;
            AvailableDemons.Clear();
            AvailableDemons.AddRange(Demons);
        }
    }
}
