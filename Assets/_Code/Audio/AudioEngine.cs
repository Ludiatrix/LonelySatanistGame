using System.Collections;
using System.Collections.Generic;
using LSG.Core;
using UnityEngine;

namespace LSG.Audio
{
    /// <summary>
    /// Audio Engine for music and sounds. Due to the overhead of audio inherent in webgl,
    /// it's often easier to pipe it through one source for optimization reasons.
    /// </summary>
    public class AudioEngine : MonoBehaviour
    {
        private readonly Queue<ClipPayload> _clipsToPlay = new Queue<ClipPayload>();
        private AudioSource _audioSource = null;
        private Coroutine _co = null;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }
        
        private void Start()
        {
            if (_co != null)
            {
                StopCoroutine(_co);
            }

            _co = StartCoroutine(AudioEngineHeartbeat_Co());
        }

        private void OnEnable()
        {
            GameEvents.PlaySoundSfx?.AddListener(OnPlaySoundSfx);
        }

        private void OnDisable()
        {
            GameEvents.PlaySoundSfx?.RemoveListener(OnPlaySoundSfx);
        }
        
        private void OnPlaySoundSfx(AudioClip soundToPlay, float volumeToPlayAt)
        {
            _clipsToPlay.Enqueue(new ClipPayload() {clip =  soundToPlay, volume = volumeToPlayAt});
        }

        private IEnumerator AudioEngineHeartbeat_Co()
        {
            WaitForSeconds wait = new WaitForSeconds(1.0f);
            
            while (true)
            {
                if (_clipsToPlay.Count > 0)
                {
                    if (_clipsToPlay.TryDequeue(out ClipPayload clipPayload))
                    {
                        _audioSource.volume = clipPayload.volume;
                        _audioSource.PlayOneShot(clipPayload.clip);
                    }
                }
                yield return wait;
            }
        }
    }
}
