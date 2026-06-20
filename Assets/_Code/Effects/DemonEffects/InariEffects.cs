using LSG.Classes;
using LSG.Core;
using LSG.Interfaces;
using UnityEngine;

namespace LSG.Effects
{
    /// <summary>
    /// Please update if changed
    /// Boon: For the rest of the game, you will see a warning (either in the dialog box or an icon in the HUD) if the next card to be drawn is white suit.
    /// Bane: For the rest of the game, the first Orange card played each round causes an additional -1 Sanity loss
    /// </summary>
    public class InariEffects : MonoBehaviour, IEffectable
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