using UnityEngine.Events;

namespace LSG.Core
{
    /// <summary>
    /// This static class is the primary communicator and bus for our phases.
    /// Primarily this is used for our boon and bane effects.
    /// </summary>
    public static class PhaseEvents
    {
        // Title Phase Events
        public static readonly UnityEvent TitlePhaseStarted = new UnityEvent();
        public static readonly UnityEvent TitlePhaseEnded = new UnityEvent();
        
        // Setup Phase Events
        public static readonly UnityEvent SetupPhaseStarted = new UnityEvent();
        public static readonly UnityEvent SetupPhaseEnded = new UnityEvent();
        
        // Summoning Phase Events
        public static readonly UnityEvent SummoningPhaseStarted = new UnityEvent();
        public static readonly UnityEvent SummoningPhaseEnded = new UnityEvent();
        
        // Encounter/Date Phase Events
        public static readonly UnityEvent EncounterPhaseStarted = new UnityEvent();
        public static readonly UnityEvent EncounterPhaseEnded = new UnityEvent();
        
        // Store Phase Events
        public static readonly UnityEvent StorePhaseStarted = new UnityEvent();
        public static readonly UnityEvent StorePhaseEnded = new UnityEvent();
        
        // Boss Phase Events
        public static readonly UnityEvent BossPhaseStarted = new UnityEvent();
        public static readonly UnityEvent BossPhaseEnded = new UnityEvent();
    }
}