using System;
using LSG.Classes;
using LSG.Core;
using LSG.Interfaces;
using UnityEngine;

namespace LSG.Effects
{
    /// <summary>
    /// Please update if changed
    /// Boon: If you fail a date roll against Beelzebabe, you can try to date them again.
    /// Bane: Lose 3 Sanity
    /// </summary>
    public class BeelzebabeEffects : MonoBehaviour, IEffectable
    {
        [SerializeField] private ModifierPayload banePayload;
        
        private void Start()
        {
            banePayload = new ModifierPayload
            {
                Sanity = -3
            };
        }

        public void ApplyBoon()
        {
            // This is just set in DemonData already
        }

        public void ApplyBane()
        {
            EconomyEvents.SendPayload?.Invoke(banePayload);
        }
    }
}