using System;

namespace LSG.Classes
{
    /// <summary>
    /// Set to Positive or Negative in order to
    /// give that modifier to the Player.
    /// </summary>
    [Serializable]
    public class ModifierPayload
    {
        public int Tape = 0;
        public int Page = 0;
        public int Power = 0;
        public int Sanity = 0;
        public int Rizz = 0;
    }
}