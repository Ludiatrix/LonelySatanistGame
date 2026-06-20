using System.Collections.Generic;
using LSG.Core;
using UnityEngine;
using UnityEngine.UI;

namespace LSG.UI
{
    /// <summary>
    /// Displays Current Amount of White Suits played this
    /// Summoning Phase.
    /// </summary>
    public class WhiteSuitCanvasController : MonoBehaviour
    {
        
        [SerializeField] private Image dangerImage;
        [Tooltip("Matching index. 0 danger = entry 0")]
        [SerializeField] private List<Sprite> dangerSprites;

        private void OnEnable()
        {
            GameEvents.WhiteSuitPointEarned?.AddListener(OnWhiteSuitPointEarned);
            GameEvents.ChangeState?.AddListener(OnStateChanged);
        }

        private void OnDisable()
        {
            GameEvents.WhiteSuitPointEarned?.RemoveListener(OnWhiteSuitPointEarned);
            GameEvents.ChangeState?.RemoveListener(OnStateChanged);
        }
        
        private void OnStateChanged(Enums.GameState state)
        {
            dangerImage.sprite = dangerSprites[0];
        }
        
        private void OnWhiteSuitPointEarned(int whiteSuitPoints)
        {
            if (whiteSuitPoints < 0) whiteSuitPoints = 0;
            if (whiteSuitPoints >= dangerSprites.Count) whiteSuitPoints = dangerSprites.Count - 1;
            
            dangerImage.sprite = dangerSprites[whiteSuitPoints];
        }
    }
}