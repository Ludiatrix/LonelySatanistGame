using UnityEngine.Events;

namespace LSG.Core
{
    /// <summary>
    /// This static class is the primary communicator and bus for our cards.
    /// Primarily this is used for our boon and bane effects.
    /// </summary>
    public static class CardEvents
    {
        public static readonly UnityEventExtensions.UnityCardDataEvent CardRepaired = new UnityEventExtensions.UnityCardDataEvent();
        public static readonly UnityEventExtensions.UnityCardDataEvent CardPlayed = new UnityEventExtensions.UnityCardDataEvent();
        public static readonly UnityEventExtensions.UnitySuitEvent AddRandomCard = new UnityEventExtensions.UnitySuitEvent();
        public static readonly UnityEventExtensions.UnitySuitEvent RemoveRandomCard = new UnityEventExtensions.UnitySuitEvent();
        public static readonly UnityEventExtensions.UnityCardDataEvent BuyCardRequest = new UnityEventExtensions.UnityCardDataEvent();
        public static readonly UnityEventExtensions.UnityCardDataEvent BuyCardSuccessResponse = new UnityEventExtensions.UnityCardDataEvent();
        public static readonly UnityEventExtensions.UnityCardDataEvent BuyCardFailedResponse = new UnityEventExtensions.UnityCardDataEvent();
    }
}