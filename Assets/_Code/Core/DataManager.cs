using System;
using LSG.Effects;
using LSG.Interfaces;
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
        public EffectManager EffectDataSource;
        
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
            ResetData();
        }

        private void OnEnable()
        {
            GameEvents.StartGame?.AddListener(OnStartGame);
        }

        private void OnDisable()
        {
            GameEvents.StartGame?.RemoveListener(OnStartGame);
        }

        // A new game/run resets the per-game data. Milestones are a "once per game"
        // unlock economy, so their collected flags must clear so tape can be earned
        // again (and their bar markers re-show).
        private void OnStartGame(Enums.GameState _)
        {
            MilestoneDataSource.Reset();
        }

        public void ResetData()
        {
            PlayerEconomySource.Reset();
            MilestoneDataSource.Reset();
            PlayerDeckSource.Reset();
            DemonDatingPoolSource.Reset();
            
            // it's easier to just blow away and recreate this
            Destroy(gameObject.GetComponent<EffectManager>());
            EffectDataSource = gameObject.AddComponent<EffectManager>();

            foreach(IEffectable demonEffectComponent in gameObject.GetComponents<IEffectable>())
            {
                Destroy((UnityEngine.Object) demonEffectComponent);
            }
        }
    }
}
