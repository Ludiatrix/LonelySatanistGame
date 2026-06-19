using System;
using LSG.Classes;
using LSG.Core;
using LSG.Interfaces;
using UnityEngine;

namespace LSG.Effects
{
    /// <summary>
    /// Please update if changed
    /// Boon: Add a random orange card that has not yet been purchased to the deck. 
    /// Bane: TODO: Write a bane for Coldkatta!
    /// </summary>
    public class ColdkattaEffects : MonoBehaviour, IEffectable
    {

        public void ApplyBoon()
        {
            CardEvents.AddRandomCard?.Invoke(Enums.Suit.Orange, true);
        }

        public void ApplyBane()
        {
            
        }
    }
}