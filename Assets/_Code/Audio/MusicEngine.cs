using UnityEngine;
using LSG.Core;

namespace LSG
{
    public class MusicEngine : MonoBehaviour
	{
        private AudioSource _audioSource = null;
		
        public AudioClip SummoningPhase, EncounterPhase, Lose;
		
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
		{
            GameEvents.ChangeState.AddListener(OnChangeState);
		}

        private void OnChangeState(Enums.GameState phase)
        {
            _audioSource.Stop();
            switch (phase)
            {
                case Enums.GameState.SummoningPhase:
                    _audioSource.clip = SummoningPhase;
                    _audioSource.Play();
                    break;
                case Enums.GameState.EncounterPhase:
                    _audioSource.clip = EncounterPhase;
                    _audioSource.Play();
                    break;
                case Enums.GameState.LosePhase:
                    _audioSource.PlayOneShot(Lose);
                    break;
            }
        }
	}
}
