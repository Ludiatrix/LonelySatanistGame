using System;
using LSG.Core;
using LSG.ScriptableObjects;
using LSG.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace LSG.UI
{
    /// <summary>
    /// Applies visuals to pages.
    /// </summary>
    public class PageFacade : MonoBehaviour
    {
        [SerializeField] private SmoothRotator pageRotator;
        [SerializeField] private Image pageImage;
        public CardData cardData = null;
        [SerializeField] private Sprite blankPageSprite;
        private Transform _pageTurnDestinationTransform;
        
        public void Inject(CardData data, Transform PageTurnDestinationTransform = null)
        {
            cardData = data;
            _pageTurnDestinationTransform = PageTurnDestinationTransform;
            ApplyVisuals();
        }

        private void ApplyVisuals()
        {
            UIEvents.SetNamePlateText?.Invoke(cardData.CardWord);
            UIEvents.SetDialogueText?.Invoke(cardData.CardEffect);
            pageImage.sprite = cardData.SummoningPage;
        }

        private void RunPageAnimation(Transform PageTurnDestinationTransform)
        {
            pageRotator.RotateToTarget(PageTurnDestinationTransform.eulerAngles, 1.0f, SwapToBlankPage);
        }

        public void SwapToBlankPage()
        {
            pageImage.sprite = blankPageSprite;
            transform.SetAsLastSibling();
        }

        internal void TurnPage()
        {
            RunPageAnimation(_pageTurnDestinationTransform);
        }
    }
}