using UnityEngine.Events;

namespace LSG
{
    /// <summary>
    /// This static class is the primary communicator and bus for our observer pattern. All our events and actions should probably go through this so it's easier to find bottlenecks and issues :3
    /// </summary>
    public static class GameEvents
    {
        public static readonly UnityEvent<Enums.GameState> StartGame = new UnityEvent();
    }
    
    
    public class UnityEvent : UnityEvent<Enums.GameState> { }
}
