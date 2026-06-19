using LSG.Core;

namespace LSG.Interfaces
{
    /// <summary>
    /// Interface for Effects. Usually pulls from listening to an Event or applies an immediate change.
    /// </summary>
    public interface IEffectable
    {
        void ApplyBoon();
        void ApplyBane();
    }
}