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
        public PlayerEconomy PlayerEconomy;
        public TMP_Text StatText;

        private string _statisticsMessage = String.Empty;

        private void Update()
        {
            ConstructStatisticsMessage();
        }

        private void ConstructStatisticsMessage()
        {
            StartStatisticsMessage();
            AddToStatistics("Current Phase",GameController.CurrentPhase.ToString());
            AddToStatistics("Tape",PlayerEconomy.Tape.ToString());
            AddToStatistics("Page",PlayerEconomy.Page.ToString());
            AddToStatistics("Power",PlayerEconomy.Power.ToString());
            AddToStatistics("Sanity",PlayerEconomy.Sanity.ToString());
            AddToStatistics("Rizz",PlayerEconomy.Rizz.ToString());

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
