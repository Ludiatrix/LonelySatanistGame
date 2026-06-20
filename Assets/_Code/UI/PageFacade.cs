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
        private CardData _cardData = null;
        
        public void Inject(CardData data, Transform PageTurnDestinationTransform)
        {
            _cardData = data;
            ApplyVisuals();
            RunPageAnimation(PageTurnDestinationTransform);
        }

        private void ApplyVisuals()
        {
            UIEvents.SetNamePlateText?.Invoke(_cardData.CardWord);
            UIEvents.SetDialogueText?.Invoke(_cardData.CardEffect);
            pageImage.sprite = _cardData.PageImage;
        }

        private void RunPageAnimation(Transform PageTurnDestinationTransform)
        {
            pageRotator.RotateToTarget(PageTurnDestinationTransform.eulerAngles, 1.0f);
        }
    }
}