using System;
using System.Collections.Generic;
using LSG.Core;
using LSG.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LSG.UI
{
    /// <summary>
    /// Drives the "peek at the next pages" UI used by the black/eye card effects. The dialogue
    /// box shows the triggering page's instruction; clicking a peeked page previews its power
    /// text; the two "Read [word] Page" buttons read that page (a full page read), and "Done"
    /// (or hitting the read limit) returns to the summoning flow.
    /// </summary>
    public class PickACardCanvasController : MonoBehaviour
    {
        public GameObject container;
        public GameObject horizontalContainer;
        public List<GameObject> pickablePages = new List<GameObject>();
        public GameObject pagePrefab;

        [Header("Peek buttons")]
        [Tooltip("Reads the left (next-to-draw) peeked page.")]
        public Button readLeftButton;
        public TMP_Text readLeftLabel;
        [Tooltip("Reads the right peeked page.")]
        public Button readRightButton;
        public TMP_Text readRightLabel;
        [Tooltip("Leaves the peek and returns to the summoning flow.")]
        public GameObject doneButton;

        // Dirty hack to turn the necronomicon on/off for our purposes
        public GameObject SummoningContainer;

        private PickACardPayload _pickACardPayload;
        private readonly List<CardData> _readCards = new List<CardData>();

        void OnEnable()
        {
            GameEvents.PickACard?.AddListener(OnPickACard);
        }

        void OnDisable()
        {
            GameEvents.PickACard?.RemoveListener(OnPickACard);
        }

        private void OnPickACard(PickACardPayload payload)
        {
            container.SetActive(true);
            _pickACardPayload = payload;
            _readCards.Clear();

            UIEvents.ToggleDialogueWindow?.Invoke(true);
            UIEvents.ToggleResourceUI?.Invoke(false);
            UIEvents.DisableButtons?.Invoke();
            SummoningContainer.SetActive(false);

            foreach (var card in payload.Cards)
            {
                GeneratePickablePage(card);
            }

            ConfigureReadButtons();
            if (doneButton != null) doneButton.SetActive(true);

            // Set the dialogue last so it wins over the per-page text each PageFacade.Inject
            // writes as it spawns. Until the player previews (selects) a peeked page, the box
            // shows the triggering black page's instruction.
            ShowInstructionText();
        }

        private void GeneratePickablePage(CardData card)
        {
            var page = Instantiate(pagePrefab, horizontalContainer.transform);

            page.GetComponent<PageFacade>().Inject(card);
            page.GetComponent<Button>().onClick.AddListener(() => SelectForPreview(card));
            pickablePages.Add(page);
        }

        /// <summary>Clicking a peeked page previews its power text in the dialogue box.</summary>
        public void SelectForPreview(CardData data)
        {
            UIEvents.SetNamePlateText?.Invoke(data.CardWord);
            UIEvents.SetDialogueText?.Invoke(data.CardEffect);
        }

        /// <summary>
        /// Wired to the panel background: clicking off a page deselects it, so the dialogue
        /// reverts to the triggering page's instruction text.
        /// </summary>
        public void ClearCardDescription()
        {
            ShowInstructionText();
        }

        private void ShowInstructionText()
        {
            UIEvents.SetNamePlateText?.Invoke("Smol");
            UIEvents.SetDialogueText?.Invoke(InstructionText());
        }

        /// <summary>The triggering page's effect text, minus the shared "Peek..." preamble.</summary>
        private string InstructionText()
        {
            string effect = _pickACardPayload?.SourceCard != null
                ? _pickACardPayload.SourceCard.CardEffect
                : _pickACardPayload?.Reason ?? string.Empty;

            const string marker = "Peek at the next 2 pages.";
            int idx = effect.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
            if (idx >= 0)
            {
                return effect.Substring(idx + marker.Length).TrimStart();
            }

            return effect;
        }

        private void ConfigureReadButtons()
        {
            CardData left = _pickACardPayload.Cards.Count > 0 ? _pickACardPayload.Cards[0] : null;
            CardData right = _pickACardPayload.Cards.Count > 1 ? _pickACardPayload.Cards[1] : null;

            SetupReadButton(readLeftButton, readLeftLabel, left);
            SetupReadButton(readRightButton, readRightLabel, right);
        }

        private void SetupReadButton(Button button, TMP_Text label, CardData card)
        {
            if (button == null) return;

            if (card == null)
            {
                // Fewer upcoming pages than expected (e.g. small deck): hide the spare button.
                button.gameObject.SetActive(false);
                return;
            }

            button.gameObject.SetActive(true);
            button.interactable = true;
            if (label != null) label.text = $"Read {card.CardWord} Page";
        }

        // Hooked to the two Read buttons in the prefab.
        public void ReadLeft() => ReadCardAt(0);
        public void ReadRight() => ReadCardAt(1);

        private void ReadCardAt(int index)
        {
            if (_pickACardPayload == null) return;
            if (index < 0 || index >= _pickACardPayload.Cards.Count) return;

            CardData card = _pickACardPayload.Cards[index];
            if (card == null || _readCards.Contains(card)) return;

            // Reading a peeked page is a full page read: power to the meter, run its own
            // effect, and move it out of the upcoming deck into played cards.
            FullyReadCard(card);
            _readCards.Add(card);

            // Disable the button for the page just read; the other read button (Black 3) and
            // Done stay available.
            Button button = index == 0 ? readLeftButton : readRightButton;
            if (button != null) button.interactable = false;

            // Reading deselects, so the dialogue returns to the instruction text.
            ShowInstructionText();

            // Black 1 & 2 allow a single read (finish at once); Black 3 allows two. Also finish
            // if there are no more pages left to read.
            if (_readCards.Count >= _pickACardPayload.AmountAbleToChoose ||
                _readCards.Count >= _pickACardPayload.Cards.Count)
            {
                Close();
            }
        }

        private void FullyReadCard(CardData card)
        {
            EconomyEvents.SendPayload?.Invoke(card.PageModifier);
            DataManager.Instance.EffectDataSource.ResolveCardEffect(card);
            DataManager.Instance.PlayerDeckSource.MarkCardRead(card);
        }

        /// <summary>Hooked to Done, and called automatically once the read limit is reached.</summary>
        public void Close()
        {
            if (_pickACardPayload == null) return;

            ResolveAfterEffects(_pickACardPayload.StateAfterChoice);

            UIEvents.ToggleResourceUI?.Invoke(true);
            _pickACardPayload = null;
            _readCards.Clear();

            foreach (var page in pickablePages)
            {
                Destroy(page);
            }
            pickablePages.Clear();

            container.SetActive(false);
            UIEvents.ToggleSummoningButtons?.Invoke(true);
            SummoningContainer.SetActive(true);
        }

        private void ResolveAfterEffects(Enums.PickACardAfterEffectState stateAfterChoice)
        {
            switch (stateAfterChoice)
            {
                case Enums.PickACardAfterEffectState.ShuffleRestIntoDeck:
                    ShuffleRest();
                    break;
                case Enums.PickACardAfterEffectState.SetAsideTheRest:
                    SetAsideRest();
                    break;
                case Enums.PickACardAfterEffectState.PutOnBottom:
                    PutOnBottom();
                    break;
                default:
                    Debug.LogWarning("[PickACard] Unknown after-effect state.");
                    break;
            }
        }

        // Unread peeked pages were never pulled from the deck, so just reshuffle.
        private void ShuffleRest()
        {
            DataManager.Instance.PlayerDeckSource.playerDeck.Shuffle();
        }

        // Unread peeked pages are pulled from the deck and held until the next summoning
        // (re-added by SetAsideCanvasController on SummoningPhaseStarted).
        private void SetAsideRest()
        {
            foreach (var card in _pickACardPayload.Cards)
            {
                if (!_readCards.Contains(card))
                {
                    DataManager.Instance.PlayerDeckSource.playerDeck.Remove(card);
                    GameEvents.PlaceCardsInSetAsideArea?.Invoke(card);
                }
            }
        }

        // Unread peeked pages go to the bottom of the book (drawn last). Draw is from the end
        // of the list, so the bottom of the deck is index 0.
        private void PutOnBottom()
        {
            var deck = DataManager.Instance.PlayerDeckSource.playerDeck;
            foreach (var card in _pickACardPayload.Cards)
            {
                if (!_readCards.Contains(card))
                {
                    deck.Remove(card);
                    deck.Insert(0, card);
                }
            }
        }
    }
}
