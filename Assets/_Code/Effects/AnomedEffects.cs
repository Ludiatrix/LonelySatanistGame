using System;
using LSG.Classes;
using LSG.Core;
using LSG.Interfaces;
using UnityEngine;

namespace LSG.Effects
{
    /// <summary>
    /// Please update if changed
    /// Boon: Trade Sanity and Rizz stat scores
    /// Bane: The next summoning, the dialog box is flipped horizontally (so text appears mirrored)
    /// </summary>
    public class AnomedEffects : MonoBehaviour, IEffectable
    {
        [SerializeField] private ModifierPayload boonPayload;

        private bool _nextSummoningComplete = false;

        public void ApplyBoon()
        {
            var economy = DataManager.Instance.PlayerEconomySource;
            boonPayload.Sanity = economy.Rizz;
            boonPayload.Rizz = economy.Sanity;
            EconomyEvents.SendPayload?.Invoke(boonPayload);
        }

        public void ApplyBane()
        {
            
        }
    }
}