using LSG.Classes;
using LSG.Core;
using LSG.Interfaces;
using UnityEngine;

namespace LSG.Effects
{
    /// <summary>
    /// Please update if changed
    /// Boon: TODO: Write a boon for Hoa
    /// Bane: One of your non-white pages is randomly removed and cannot be purchased again this game.
    /// </summary>
    public class HoaEffects : MonoBehaviour, IEffectable
    {
        [SerializeField] private ModifierPayload boonPayload;
        [SerializeField] private ModifierPayload banePayload;

        private void Start()
        {
            boonPayload = new ModifierPayload
            {
                Power = 2
            };
            
            banePayload = new ModifierPayload
            {
                Sanity = -1
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