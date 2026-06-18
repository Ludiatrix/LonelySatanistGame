using LSG.Core;
using UnityEngine;

namespace LSG.UI
{
    /// <summary>
    /// Displays Current Amount of White Suits played this
    /// Summoning Phase.
    /// </summary>
    public class WhiteSuitCanvasController : MonoBehaviour
    {
        [SerializeField] private GameObject[] whiteSuitWarningImages;

        private void OnEnable()
        {
            GameEvents.WhiteSuitPointEarned?.AddListener(OnWhiteSuitPointEarned);
            GameEvents.ChangeState?.AddListener(OnStateChanged);
            ToggleImages(false); // make sure everyone is off
        }

        private void OnDisable()
        {
            GameEvents.WhiteSuitPointEarned?.RemoveListener(OnWhiteSuitPointEarned);
            GameEvents.ChangeState?.RemoveListener(OnStateChanged);
        }
        
        private void OnStateChanged(Enums.GameState state)
        {
            ToggleImages(false);
        }
        
        private void OnWhiteSuitPointEarned(int whiteSuitPoints)
        {
            for (int i = 0; i < whiteSuitPoints; i++)
            {
                whiteSuitWarningImages[i].SetActive(true);
            }
        }

        private void ToggleImages(bool toggle)
        {
            foreach (var whiteSuitWarningImage in whiteSuitWarningImages)
            {
                whiteSuitWarningImage.SetActive(toggle);
            }
        }
    }
}