using System;

namespace LSG.TapeSystem
{
    /// <summary>
    /// A Milestone is an arbitrary marker set on the powerbar that holds tape.
    /// Once tape has been collected, it cannot be recollected unless explicitly
    /// changed by an effect.
    /// </summary>
    [Serializable]
    public class Milestone
    {
        public int PowerLevel = 1;
        public int TapeAmount = 1;
    }
}