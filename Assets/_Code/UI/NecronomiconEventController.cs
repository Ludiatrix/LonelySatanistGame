using System;
using System.Collections.Generic;
using LSG.Core;
using LSG.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

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

        // Page Generation
        public Transform PagesTransform;
        public GameObject PagePrefab;
        public Transform PageTurnDestinationTransform;

        [SerializeField] private Image[] imagesToToggle;

        public List<GameObject> populatedPages = new List<GameObject>();

        private PageFacade _currentPage = null;

        private Animator _animator = null;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            GameEvents.StartReading?.AddListener(OnStartReading);
            GameEvents.NecronomiconStartSliding?.AddListener(OnNecronomiconStartSliding);
            UIEvents.DisplayNecronomiconPage?.AddListener(OnDisplayNecronomiconPage);
            UIEvents.TurnNecronomiconPage?.AddListener(OnTurnNecronomiconPage);
            PhaseEvents.SetupPhaseStarted?.AddListener(ResetNecronomicon);
            UIEvents.ToggleNecronomicon?.AddListener(ToggleNecronomicon);
            PhaseEvents.SummoningPhaseEnded?.AddListener(OnSummoningPhaseEnded);
        }

        private void ToggleNecronomicon(bool toggle)
        {
            _animator.enabled = toggle;
            foreach (var image in imagesToToggle)
            {
                image.enabled = toggle;
            }

        }

        private void ResetNecronomicon()
        {
            _animator.enabled = false;
            _animator.ResetControllerState(true); 
            _animator.enabled = true;
            ToggleNecronomicon(true);
            _animator.Play(SlideUp);
        }

        private void OnSummoningPhaseEnded()
        {
            ToggleNecronomicon(false);
            foreach (var page in populatedPages)
            {
                Destroy(page);
            }
            populatedPages.Clear();
        }

        private void OnDisplayNecronomiconPage(CardData data)
        {
            DisplayPage(data);
        }

        private void OnTurnNecronomiconPage()
        {
            _currentPage?.TurnPage();
            _currentPage = null;
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

        private void DisplayPage(CardData data)
        {
            GameObject page = Instantiate(PagePrefab, PagesTransform, false);
            populatedPages.Add(page);
            page.transform.SetAsFirstSibling();
            _currentPage = page.GetComponent<PageFacade>();
            _currentPage.Inject(data, PageTurnDestinationTransform);
        }

        public void DoneOpening()
        {
            GameEvents.ChangeState?.Invoke(Enums.GameState.SummoningPhase);
        }
    }
}
