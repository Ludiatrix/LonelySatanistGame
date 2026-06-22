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
        
        public void Inject(CardData data, Transform PageTurnDestinationTransform = null)
        {
            cardData = data;
            ApplyVisuals();
            RunPageAnimation(PageTurnDestinationTransform);
        }

        private void ApplyVisuals()
        {
            UIEvents.SetNamePlateText?.Invoke(cardData.CardWord);
            UIEvents.SetDialogueText?.Invoke(cardData.CardEffect);
            pageImage.sprite = cardData.SummoningPage;
        }

        private void RunPageAnimation(Transform PageTurnDestinationTransform)
        {
            pageRotator.RotateToTarget(PageTurnDestinationTransform.eulerAngles, 1.0f);
        }
    }
}