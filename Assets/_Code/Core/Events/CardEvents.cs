using UnityEngine.Events;

namespace LSG.Core
{
    /// <summary>
    /// This static class is the primary communicator and bus for our cards.
    /// Primarily this is used for our boon and bane effects.
    /// </summary>
    public static class CardEvents
    {
        public static readonly UnityEventExtensions.UnityPageDataEvent CardRepaired = new UnityEventExtensions.UnityPageDataEvent();
        public static readonly UnityEventExtensions.UnityPageDataEvent CardPlayed = new UnityEventExtensions.UnityPageDataEvent();
        public static readonly UnityEventExtensions.UnitySuitEvent AddRandomCard = new UnityEventExtensions.UnitySuitEvent();
    }
}