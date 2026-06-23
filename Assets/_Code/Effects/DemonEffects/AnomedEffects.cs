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

        private bool BaneEnabled = true;

        private void OnEnable()
        {
            PhaseEvents.SummoningPhaseStarted?.AddListener(OnSummoningPhaseStarted);
			PhaseEvents.SummoningPhaseEnded?.AddListener(OnSummoningPhaseEnded);
        }

        private void Start()
        {
            boonPayload = new ModifierPayload();
        }

		private void OnSummoningPhaseEnded()
		{
			UIEvents.FlipDialogueText?.Invoke(false);
		}

        private void OnSummoningPhaseStarted()
        {
            if (!BaneEnabled){
				//this was somehow persisting even through summoning phase ended
				//assuming there was some issue with the event busses and just throwing it here too. 
				UIEvents.FlipDialogueText?.Invoke(false);
				return;
			}
            ApplyBane();
        }

        public void ApplyBoon()
        {
            var economy = DataManager.Instance.PlayerEconomySource;
            boonPayload.Sanity = economy.Rizz;
            boonPayload.Rizz = economy.Sanity;
            EconomyEvents.SendPayload?.Invoke(boonPayload);
        }

        public void ApplyBane()
        {
            BaneEnabled = false;
            UIEvents.FlipDialogueText?.Invoke(true);
        }
    }
}