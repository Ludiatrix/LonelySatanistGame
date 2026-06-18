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

        private PageData _pageData = null;
        
        public void Inject(PageData data, Transform PageTurnDestinationTransform)
        {
            _pageData = data;
            RunPageAnimation(PageTurnDestinationTransform);
        }
        
        private void RunPageAnimation(Transform PageTurnDestinationTransform)
        {
            pageRotator.RotateToTarget(PageTurnDestinationTransform.eulerAngles, 1.0f);
        }
    }
}