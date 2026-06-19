using LSG.ScriptableObjects;
using LSG.Utils;
using UnityEngine;

namespace LSG.UI
{
    /// <summary>
    /// Applies visuals to pages.
    /// </summary>
    public class PageFacade : MonoBehaviour
    {
        [SerializeField] private SmoothRotator pageRotator;

        private CardData _cardData = null;
        
        public void Inject(CardData data, Transform PageTurnDestinationTransform)
        {
            _cardData = data;
            ApplyVisuals();
            RunPageAnimation(PageTurnDestinationTransform);
        }

        private void ApplyVisuals()
        {
            // Purely because effects can screw with visuals we have a
            
        }

        private void RunPageAnimation(Transform PageTurnDestinationTransform)
        {
            pageRotator.RotateToTarget(PageTurnDestinationTransform.eulerAngles, 1.0f);
        }
    }
}