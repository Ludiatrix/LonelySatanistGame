using System;
using LSG.Classes;
using LSG.Core;
using LSG.Interfaces;
using UnityEngine;

namespace LSG.Effects
{
    /// <summary>
    /// Please update if changed
    /// Boon: TODO: Write a boon for the Admiral!
    /// Bane: Lose 1 Tape (if the player has a tape banked)
    /// </summary>
    public class AdmiralFortranEffects : MonoBehaviour, IEffectable
    {
        [SerializeField] private ModifierPayload boonPayload;
        [SerializeField] private ModifierPayload banePayload;

        private void Start()
        {
            boonPayload = new ModifierPayload
            {
                Tape = 1
            };
            
            banePayload = new ModifierPayload
            {
                Sanity = -2
            };
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