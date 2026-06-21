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
            StorePhase = 5,
            WinPhase = 7,
            LosePhase = 8,
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
