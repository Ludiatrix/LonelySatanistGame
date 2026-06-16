using LSG.ScriptableObjects;
using LSG.TapeSystem;
using UnityEngine;

namespace LSG.Phases
{
    /*
     * TODO: Pages are drawn one by one. The player chooses to "Keep Reading" to gain more power/tape milestones or "Stop" to lock in their power meter.
     * If they exceed the safe threshold of the White suit, their summoning attempt calls the dog and the player gets dragged to Hell (bad ending).
     */
    
    /// <summary>
    /// The Summoning Phase is governed by two primary actions: "Keep Reading" and "Stop". This is the most direct interpretation of a calculated risk!
    /// "Keep Reading" uses the ReadPage function, which uses the current milestone and sends
    /// </summary>
    public class SummoningPhase : Phase
    {
        /// <summary>
        /// Reading a Page causes the PageRead event to fire in GameEvents.cs which mainly adds to PlayerEconomy.
        /// </summary>
        public void ReadPage()
        {
            Debug.Log("[Summoning Phase] Reading Page...");
            GameEvents.PageRead?.Invoke();
        }

        /// <summary>
        /// Stopping locks in the Power meter and performs the act of Summoning.
        /// TODO: Conduct Summoning!
        /// </summary>
        public void StopReading()
        {
            Debug.Log("[Summoning Phase] The Pages have been read. Now let's see what the chasm of hell brings forth!");
        }
        
    }
}
