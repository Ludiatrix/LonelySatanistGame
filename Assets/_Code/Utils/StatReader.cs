using System;
using LSG.Core;
using TMPro;
using UnityEngine;

namespace LSG.Utils
{
    /// <summary>
    /// Handy little stat reader for our economy and other doodads.
    /// </summary>
    public class StatReader : MonoBehaviour
    {
        public GameController GameController;
        private PlayerEconomy _economy;
        public TMP_Text StatText;

        private string _statisticsMessage = String.Empty;

        private void Start()
        {
            _economy = DataManager.Instance.PlayerEconomySource;
        }

        private void Update()
        {
            ConstructStatisticsMessage();
        }

        private void ConstructStatisticsMessage()
        {
            StartStatisticsMessage();
            AddToStatistics("Current Phase",GameController.CurrentPhase.ToString());
            AddToStatistics("Tape",_economy.Tape.ToString());
            AddToStatistics("Page",_economy.Page.ToString());
            AddToStatistics("Power",_economy.Power.ToString());
            AddToStatistics("Sanity",_economy.Sanity.ToString());
            AddToStatistics("Rizz",_economy.Rizz.ToString());

            StatText.text = _statisticsMessage;
        }

        private void StartStatisticsMessage()
        {
            _statisticsMessage = "Statistics \n";
        }
        
        private void AddToStatistics(string statName, string statValue)
        {
            _statisticsMessage += $"{statName}:{statValue}\n";
        }
    }
}
