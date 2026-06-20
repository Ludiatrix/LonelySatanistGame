using LSG.Classes;
using LSG.Core;
using LSG.Interfaces;
using UnityEngine;

namespace LSG.Effects
{
    /// <summary>
    /// Please update if changed
    /// Boon: If the next 2 cards are a black suit card and a white suit card, the black suit card will move to the position of the next to be drawn.
    /// Bane: For the rest of the game, all pages are shuffled back into the book after they are played (but power is kept)
    /// </summary>
    public class YaldabaothEffects : MonoBehaviour, IEffectable
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