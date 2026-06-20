using System.Collections;
using LSG.Core;
using LSG.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace LSG
{
    /// <summary>
    /// Manages the shared Card Description box as a click-driven toggle. Place this
    /// on the store canvas (or near the box) and assign the box.
    ///
    /// - A card calls Toggle(owner) when clicked: opens the box for that card,
    ///   switches to a different card, or closes it if the same card is clicked again.
    /// - Close() can be wired to a dedicated close button (e.g. KeepReadingButton).
    ///
    /// The box is enabled after 'openDelay' seconds so it doesn't overlap the Card
    /// Grid while it slides up — onOpened still fires immediately to start the slide.
    ///
    /// Drives visibility purely from OnClick, so clicking the box itself never
    /// closes it (unlike the old Select/Deselect EventTrigger approach).
    /// </summary>
    public class CardDescriptionPanel : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("The shared Card description box to show/hide.")]
        [SerializeField] private GameObject cardDescriptionBox;

        [SerializeField] private TMP_Text cardName;
        [SerializeField] private TMP_Text cardText;
        
        [Tooltip("The text field for the button")]
        [SerializeField] private TMP_Text buttonText;

        [Header("Timing")]
        [Tooltip("Delay before the box is enabled, so it doesn't overlap the Card Grid while it slides up. Match the slide duration.")]
        [SerializeField] private float openDelay = 0.7f;

        [Header("Events (optional)")]
        [Tooltip("Fired immediately when the box opens/switches to a card (before the delay). Hook this to start the Card Grid slide-up.")]
        public UnityEvent<CardData> onOpened;
        public UnityEvent onClosed;

        // The card the box is currently showing for (null when closed).
        private CardData currentOwner;
        // Intended open state — tracked separately from activeSelf because the
        // box is enabled on a delay, so activeSelf lags behind during the slide.
        private bool isOpen;
        private Coroutine showRoutine;

        public bool IsOpen => isOpen;

        /// <summary>
        /// Called by a card when clicked. Same card again → close; different card → switch.
        /// </summary>
        public void Toggle(CardData owner)
        {
            if (cardDescriptionBox == null)
                return;

            // Clicking the same card that's already showing closes the box.
            if (isOpen && ReferenceEquals(currentOwner, owner))
            {
                Close();
                return;
            }

            // Otherwise open (or switch to) this card.
            currentOwner = owner;

            cardName.text = owner.CardWord;
            cardText.text = owner.CardEffect;
            buttonText.text = $"Repair for {owner.TapeCost.ToString()}";
            
            isOpen = true;
            onOpened.Invoke(owner);

            // Enable the box after the delay (unless it's already visible, e.g. switching cards).
            if (!cardDescriptionBox.activeSelf && showRoutine == null)
                showRoutine = StartCoroutine(ShowAfterDelay());
        }

        /// <summary>
        /// Closes the box. Wire this to a close button's OnClick.
        /// </summary>
        public void Close()
        {
            // Cancel a pending delayed-show so the box doesn't pop in after closing.
            if (showRoutine != null)
            {
                StopCoroutine(showRoutine);
                showRoutine = null;
            }

            if (cardDescriptionBox != null)
                cardDescriptionBox.SetActive(false);

            currentOwner = null;
            isOpen = false;
            onClosed.Invoke();
        }

        private IEnumerator ShowAfterDelay()
        {
            if (openDelay > 0f)
                yield return new WaitForSecondsRealtime(openDelay);

            // Guard: a Close() during the wait clears the routine and intent.
            if (isOpen && cardDescriptionBox != null)
                cardDescriptionBox.SetActive(true);

            showRoutine = null;
        }

        public void RequestToBuyCurrentlyOwnedCard()
        {
            CardEvents.BuyCardRequest?.Invoke(currentOwner);
        }
    }
}
