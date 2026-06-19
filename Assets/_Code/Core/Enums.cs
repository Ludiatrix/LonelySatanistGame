using UnityEngine;

namespace LSG.Core
{
    public class Enums
    {
        /// <summary>
        /// A collection of enums the project uses for quick referencing.
        /// </summary>
        public enum GameState
        {
            TitlePhase = 0,
            SetupPhase = 1,
            SummoningPhase = 2,
            EncounterPhase = 3,
            
            /*
             * TODO:The player decides to "Try to Date" or "Give Up". Choosing to date triggers a D20 roll against their cumulative Rizz stat.
               Success: Triggers a unique, positive single-screen Date Ending dialogue.
               Failure: The player receives the demon's Boon and Bane including any mechanical effects, (e.g. additional Sanity loss), and advances to the Shop phase.
             */
            DatePhase = 4,
            
            // TODO: The player spends earned tape to assemble additional torn up Necronomicon pages to permanently add new pages into their deck.
            StorePhase = 5,
            
            // TODO: The player hits "Summon Again," shuffling the newly expanded deck to begin the cycle anew.
            ResetPhase = 6,
            
            // TODO: The player hits the boss fight with "Beelzebabe" WIP
            BossPhase = 7,
            
            //TODO: End Phase WIP
            EndPhase = 8,
            
            NullPhase = 999,
        }

        public enum Suit
        {
            White = 0,
            Orange = 1,
            Blue = 2,
            Black = 3,
            Red = 4,
            None = 99,
        }

        public enum GameResource
        {
            Tape = 0,
            Sanity = 1,
            Rizz = 2,
            WhiteSuits = 3,
            Power = 4,
            Page = 5,
        }

        public enum EffectState
        {
            UsableOnce = 0,
            Repeatable = 1,
        }
    }
}
