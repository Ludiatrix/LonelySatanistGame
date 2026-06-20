using LSG.Classes;
using LSG.Core;
using LSG.Interfaces;
using UnityEngine;

namespace LSG.Effects
{
    /// <summary>
    /// Please update if changed
    /// Boon: Gain 1 Tape
    /// Bane: Lose 2 Sanity
    /// </summary>
    public class TentacularaEffects : MonoBehaviour, IEffectable
    {
        [SerializeField] private ModifierPayload boonPayload;
        [SerializeField] private ModifierPayload banePayload;

        private void Start()
        {
            boonPayload.Tape = 1;
            banePayload.Sanity = -2;
        }

        public void ApplyBoon()
        {
            EconomyEvents.SendPayload?.Invoke(boonPayload);
        }

        public void ApplyBane()
        {
            
            EconomyEvents.SendPayload?.Invoke(banePayload);
        }
    }
}