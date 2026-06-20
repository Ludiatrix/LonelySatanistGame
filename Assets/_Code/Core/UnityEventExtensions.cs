using LSG.Classes;
using LSG.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace LSG.Core
{
    public static class UnityEventExtensions
    {
        public class UnityStateEvent : UnityEvent<Enums.GameState> { }
        public class UnityPageReadEvent : UnityEvent { }
        public class UnityCardDataEvent : UnityEvent<CardData> { }
        public class UnityDemonDataEvent : UnityEvent<DemonData> { }
        public class UnityIntEvent : UnityEvent<int> { }
        public class UnityBoolEvent : UnityEvent<bool> { }
        public class UnityStringEvent : UnityEvent<string> { }
        public class UnityModifierPayloadEvent : UnityEvent<ModifierPayload> { }
        public class UnitySuitEvent : UnityEvent<Enums.Suit,bool> { }
        /// <summary>
        /// Sends an audioclip to the SFX engine for playing and a volume to play it at. 4.0 is the standard volume.
        /// </summary>
        public class UnitySfxEvent : UnityEvent<AudioClip,float> { }
    }
}