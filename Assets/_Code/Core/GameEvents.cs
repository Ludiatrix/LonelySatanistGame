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
        public static readonly UnityStateEvent NecronomiconOpened = new UnityStateEvent();
        
        // Summoning Phase Events
        public static readonly UnityPageReadEvent PageRead = new UnityPageReadEvent();
        public static readonly UnityEvent TapeEarnedEvent = new UnityEvent();
        
        // Utility Events
        public static readonly UnitySfxEvent PlaySoundSfx = new UnitySfxEvent();
    }
    
    
    public class UnityStateEvent : UnityEvent<Enums.GameState> { }
    public class UnityPageReadEvent : UnityEvent { }
    
    /// <summary>
    /// Sends an audioclip to the SFX engine for playing and a volume to play it at. 4.0 is the standard volume.
    /// </summary>
    public class UnitySfxEvent : UnityEvent<AudioClip,float> { }
}
