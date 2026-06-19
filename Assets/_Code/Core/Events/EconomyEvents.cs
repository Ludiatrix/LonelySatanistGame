namespace LSG.Core
{
    /// <summary>
    /// This static class is the primary communicator and bus for our economy.
    /// Primarily this is used for our boon and bane effects.
    /// </summary>
    public static class EconomyEvents
    {
        public static readonly UnityEventExtensions.UnityModifierPayloadEvent SendPayload = new UnityEventExtensions.UnityModifierPayloadEvent();
    }
}