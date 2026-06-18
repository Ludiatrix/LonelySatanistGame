using System;
using LSG.ScriptableObjects;
using UnityEngine;

namespace LSG.Core
{
    /// <summary>
    /// Singleton Instance Container for holding
    /// Scriptable Objects for different data containers. 
    /// </summary>
    public class DataManager : MonoBehaviour
    {
        public static DataManager Instance { get; private set; }
        
        public PlayerEconomy PlayerEconomySource;
        public MilestoneData MilestoneDataSource;
        public PlayerDeck PlayerDeckSource;
        public DemonDatingPool DemonDatingPoolSource;
        
        // Say the line, Bart.
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this);
        }

        public void ResetData()
        {
            PlayerEconomySource.Reset();
            MilestoneDataSource.Reset();
            PlayerDeckSource.Reset();
            DemonDatingPoolSource.Reset();
        }
    }
}
