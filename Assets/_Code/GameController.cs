using System;
using System.Linq;
using LSG.Phases;
using UnityEngine;

namespace LSG
{
    /// <summary>
    /// Primary Controller with the ability to go to different phases of the game. It primarily listens to commands from GameEvents.cs unless explicitly told otherwise.
    /// </summary>
    public class GameController : MonoBehaviour
    {
        public PhaseObject[] PhaseObjects;
        public Enums.GameState CurrentPhase = Enums.GameState.StartPhase;

        private void OnEnable()
        {
            GameEvents.StartGame?.AddListener(GoToPhase);
        }

        /// <summary>
        /// Goes to another phase. You can go to previous phases as well! Just not the same one.
        /// </summary>
        /// <param name="targetPhase">the phase you want to go to</param>
        public void GoToPhase(Enums.GameState targetPhase)
        {
            if (Equals(targetPhase, CurrentPhase))
                return;

            TryEndPhase(CurrentPhase);
            TryStartPhase(targetPhase);
            CurrentPhase = targetPhase;
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
