using UnityEngine;

namespace LSG.Audio
{
    /// <summary>
    /// A struct to hold clips that come into the audio engine and are waiting to be played.
    /// This is not likely to happen, but will prevent stepping on toes.
    /// </summary>
    public struct ClipPayload
    {
        public AudioClip clip;
        public float volume;
    }
}