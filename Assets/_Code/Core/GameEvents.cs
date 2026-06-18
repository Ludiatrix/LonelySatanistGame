using LSG.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace LSG.Core
{
    /// <summary>
    /// This static class is the primary communicator and bus for our observer pattern. All our events and actions should probably go through this so it's easier to find bottlenecks and issues :3
    /// </summary>
    public static class GameEvents
    {
        // Title Phase Events
        public static readonly UnityStateEvent StartGame = new UnityStateEvent();
        
        // Setup Phase Events
        public static readonly UnityEvent NecronomiconStartSliding = new UnityEvent();
        public static readonly UnityEvent NecronomiconFinishedSliding = new UnityEvent();
        public static readonly UnityEvent StartReading = new UnityEvent();
        
        // Summoning Phase Events
        public static readonly UnityPageReadEvent PageRead = new UnityPageReadEvent();
        public static readonly UnityEvent TapeEarnedEvent = new UnityEvent();
        public static readonly UnityIntEvent WhiteSuitPointEarned = new UnityIntEvent();
        public static readonly UnityEvent KeepReadingChosen = new UnityEvent();
        public static readonly UnityEvent StopChosen = new UnityEvent();
        
        // Store Phase Events
        public static readonly UnityPageDataEvent PageAdded = new UnityPageDataEvent();
        public static readonly UnityPageDataEvent PageTaken = new UnityPageDataEvent();
        
        // Encounter Phase Events
        public static readonly UnityDemonDataEvent DemonEncountered = new UnityDemonDataEvent();
        public static readonly UnityEvent TryToDateChosen = new UnityEvent();
        public static readonly UnityEvent GiveUpChosen = new UnityEvent();
        
        // Utility Events
        public static readonly UnityStateEvent ChangeState = new UnityStateEvent();
        public static readonly UnitySfxEvent PlaySoundSfx = new UnitySfxEvent();
        
        // UI Events
        public static readonly UnityBoolEvent ToggleDialogueWindow = new UnityBoolEvent();
        public static readonly UnityStringEvent SetNamePlateText = new UnityStringEvent();
        public static readonly UnityStringEvent SetDialogueText = new UnityStringEvent();
        public static readonly UnityBoolEvent ToggleSummoningButtons = new UnityBoolEvent();
        public static readonly UnityBoolEvent ToggleEncounterButtons = new UnityBoolEvent();
        public static readonly UnityBoolEvent ToggleStoreButtons = new UnityBoolEvent();
        public static readonly UnityEvent DisableButtons = new UnityEvent();
    }
    
    
    public class UnityStateEvent : UnityEvent<Enums.GameState> { }
    public class UnityPageReadEvent : UnityEvent { }
    public class UnityPageDataEvent : UnityEvent<PageData> { }
    public class UnityDemonDataEvent : UnityEvent<DemonData> { }
    public class UnityIntEvent : UnityEvent<int> { }
    public class UnityBoolEvent : UnityEvent<bool> { }
    public class UnityStringEvent : UnityEvent<string> { }
    
    
    /// <summary>
    /// Sends an audioclip to the SFX engine for playing and a volume to play it at. 4.0 is the standard volume.
    /// </summary>
    public class UnitySfxEvent : UnityEvent<AudioClip,float> { }
}
