using System;
using LSG.Core;
using UnityEngine;

namespace LSG.UI
{
    /// <summary>
    /// Animation Events for the Necronomicon.
    /// </summary>
    public class NecronomiconEventController : MonoBehaviour
    {
        private static readonly int SlideUp = Animator.StringToHash("SlideUp");
        private static readonly int SlideRight = Animator.StringToHash("SlideRight");
        public AudioClip PaperShufflingSFX,MoveBookSFX;

        private Animator _animator = null;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            GameEvents.StartReading?.AddListener(OnStartReading);
            GameEvents.NecronomiconStartSliding?.AddListener(OnNecronomiconStartSliding);
        }
        
        private void OnNecronomiconStartSliding()
        {
            _animator.SetTrigger(SlideUp);
        }

        private void OnStartReading()
        {
            _animator.SetTrigger(SlideRight);
        }

        public void PlayPapersShufflingSfx()
        {
            GameEvents.PlaySoundSfx?.Invoke(PaperShufflingSFX, 4.0f);
        }

        public void PlayMoveBookSFX()
        {
            GameEvents.PlaySoundSfx?.Invoke(MoveBookSFX, 4.0f);
        }
    }
}
