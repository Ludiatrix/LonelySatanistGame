using System;
using LSG.Core;
using LSG.ScriptableObjects;
using UnityEngine;

namespace LSG.UI
{
    /// <summary>
    /// Visual Controller for the PowerBar. Just here to look pretty and does not govern player state.
    /// For that, you want to look at SummoningPhase.cs
    /// </summary>
    public class PowerBarController : MonoBehaviour
    {
        public PlayerEconomy Economy;
        public MilestoneData MilestoneData;

        private void OnEnable()
        {
            GameEvents.TapeEarnedEvent?.AddListener(UpdateBar);
        }

        private void OnDisable()
        {
            GameEvents.TapeEarnedEvent?.RemoveListener(UpdateBar);
        }

        public void UpdateBar()
        {
            
        }
    }
}
