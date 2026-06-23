using System;
using System.Linq;
using LSG.Phases;
using UnityEngine;

namespace LSG.Core
{
    /// <summary>
    /// Primary Controller with the ability to go to different phases of the game. It primarily listens to commands from GameEvents.cs unless explicitly told otherwise.
    /// </summary>
    public class GameController : MonoBehaviour
    {
        // TODO: This is bad. Make the Phases report themselves to the GameController to prevent nullreferences.
        public PhaseObject[] PhaseObjects;
        public Enums.GameState CurrentPhase = Enums.GameState.NullPhase;

        // Guards against re-entrant phase changes (e.g. a phase change requested from
        // inside StartPhase, like a lose condition firing during an encounter).
        private bool _isTransitioning;
        private Enums.GameState? _queuedPhase;

        private void OnEnable()
        {
            GameEvents.StartGame.AddListener(GoToPhase);
            GameEvents.ChangeState.AddListener(GoToPhase);
        }

        private void OnDisable()
        {
            GameEvents.StartGame.RemoveListener(GoToPhase);
            GameEvents.ChangeState.RemoveListener(GoToPhase);
        }

        private void Start()
        {
            GoToPhase(Enums.GameState.TitlePhase);
        }

        /// <summary>
        /// Goes to another phase. You can go to previous phases as well! Just not the same one.
        /// </summary>
        /// <param name="targetPhase">the phase you want to go to</param>
        public void GoToPhase(Enums.GameState targetPhase)
        {
            // If a phase change is requested while we're already mid-transition,
            // queue it and run it once the current transition completes.
            if (_isTransitioning)
            {
                _queuedPhase = targetPhase;
                return;
            }

            Debug.Log($"{nameof(GoToPhase)}: {targetPhase}");

            _isTransitioning = true;
            TryEndPhase(CurrentPhase);
            TryStartPhase(targetPhase);
            CurrentPhase = targetPhase;
            _isTransitioning = false;

            if (_queuedPhase.HasValue)
            {
                Enums.GameState next = _queuedPhase.Value;
                _queuedPhase = null;
                GoToPhase(next);
            }
        }

        private void TryEndPhase(Enums.GameState phaseToEnd)
        {
            PhaseObject phaseObject = GetPhaseObjectFromGameState(phaseToEnd);
            phaseObject.GameObject.GetComponent<Phase>().EndPhase();
        }
        
        private void TryStartPhase(Enums.GameState phaseToStart)
        {
            PhaseObject phaseObject = GetPhaseObjectFromGameState(phaseToStart);
            phaseObject.GameObject.GetComponent<Phase>().StartPhase();
        }

        private PhaseObject GetPhaseObjectFromGameState(Enums.GameState gameState)
        {
            return PhaseObjects.FirstOrDefault(pObject => pObject.Phase == gameState);
        }
    }
}
