using LSG.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace LSG.Core
{
    /// <summary>
    /// This static class is the primary communicator and bus for one-off game logic events.
    /// </summary>
    public static class GameEvents
    {
        // Title Phase Events
        public static readonly UnityEventExtensions.UnityStateEvent StartGame = new UnityEventExtensions.UnityStateEvent();
        
        // Setup Phase Events
        public static readonly UnityEvent NecronomiconStartSliding = new UnityEvent();
        public static readonly UnityEvent NecronomiconFinishedSliding = new UnityEvent();
        public static readonly UnityEvent StartReading = new UnityEvent();
        
        // Summoning Phase Events
        public static readonly UnityEventExtensions.UnityPageReadEvent PageRead = new UnityEventExtensions.UnityPageReadEvent();
        public static readonly UnityEvent TapeEarnedEvent = new UnityEvent();
        public static readonly UnityEventExtensions.UnityIntEvent WhiteSuitPointEarned = new UnityEventExtensions.UnityIntEvent();
        public static readonly UnityEvent KeepReadingChosen = new UnityEvent();
        public static readonly UnityEvent StopChosen = new UnityEvent();
        
        // Store Phase Events
        public static readonly UnityEventExtensions.UnityPageDataEvent PageAdded = new UnityEventExtensions.UnityPageDataEvent();
        public static readonly UnityEventExtensions.UnityPageDataEvent PageTaken = new UnityEventExtensions.UnityPageDataEvent();
        
        // Encounter Phase Events
        public static readonly UnityEventExtensions.UnityDemonDataEvent DemonEncountered = new UnityEventExtensions.UnityDemonDataEvent();
        public static readonly UnityEvent TryToDateChosen = new UnityEvent();
        public static readonly UnityEvent GiveUpChosen = new UnityEvent();
        
        // Utility Events
        public static readonly UnityEventExtensions.UnityStateEvent ChangeState = new UnityEventExtensions.UnityStateEvent();
        public static readonly UnitySfxEvent PlaySoundSfx = new UnitySfxEvent();
        
        // UI Events
        public static readonly UnityEventExtensions.UnityBoolEvent ToggleDialogueWindow = new UnityEventExtensions.UnityBoolEvent();
        public static readonly UnityEventExtensions.UnityStringEvent SetNamePlateText = new UnityEventExtensions.UnityStringEvent();
        public static readonly UnityEventExtensions.UnityStringEvent SetDialogueText = new UnityEventExtensions.UnityStringEvent();
        public static readonly UnityEventExtensions.UnityBoolEvent ToggleSummoningButtons = new UnityEventExtensions.UnityBoolEvent();
        public static readonly UnityEventExtensions.UnityBoolEvent ToggleEncounterButtons = new UnityEventExtensions.UnityBoolEvent();
        public static readonly UnityEventExtensions.UnityBoolEvent ToggleStoreButtons = new UnityEventExtensions.UnityBoolEvent();
        public static readonly UnityEvent DisableButtons = new UnityEvent();
    }
    
    

    
    
    /// <summary>
    /// Sends an audioclip to the SFX engine for playing and a volume to play it at. 4.0 is the standard volume.
    /// </summary>
    public class UnitySfxEvent : UnityEvent<AudioClip,float> { }
}
