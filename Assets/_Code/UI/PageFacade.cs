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
        [SerializeField] private PlayerEconomy economy;
        [SerializeField] private SmoothRotator pageRotator;

        private PageData _pageData;
        
        public void Inject(Transform PageTurnDestinationTransform)
        {
            _pageData = economy.PlayerDeckSource.TakePage();
            RunPageAnimation(PageTurnDestinationTransform);
        }
        
        private void RunPageAnimation(Transform PageTurnDestinationTransform)
        {
            pageRotator.RotateToTarget(PageTurnDestinationTransform.eulerAngles, 1.0f);
        }
    }
}