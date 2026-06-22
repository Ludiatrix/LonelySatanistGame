using System.Collections.Generic;
using LSG.Core;
using LSG.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LSG.UI
{
    /// <summary>
    /// Displays the cumulative Power of White (Daggers) pages read this
    /// Summoning Phase.
    /// </summary>
    public class WhiteSuitCanvasController : MonoBehaviour
    {

        [SerializeField] private Image dangerImage;
        [Tooltip("Matching index. 0 danger = entry 0")]
        [SerializeField] private List<Sprite> dangerSprites;

        [SerializeField] private TMP_Text text;

        private int _whitePowerRead;

        private void OnEnable()
        {
            GameEvents.CardTaken?.AddListener(OnCardTaken);
            GameEvents.ChangeState?.AddListener(OnStateChanged);
        }

        private void OnDisable()
        {
            GameEvents.CardTaken?.RemoveListener(OnCardTaken);
            GameEvents.ChangeState?.RemoveListener(OnStateChanged);
        }

        private void OnStateChanged(Enums.GameState state)
        {
            // White power is per-summoning; reset whenever we change phase.
            _whitePowerRead = 0;
            ShowDanger();
        }

        private void OnCardTaken(CardData takenCard)
        {
            if (takenCard == null) return;
            if (takenCard.Suit != Enums.Suit.White) return;

            if (takenCard.PageModifier != null)
            {
                _whitePowerRead += takenCard.PageModifier.Power;
            }

            ShowDanger();
        }

        private void ShowDanger()
        {
            int spriteIndex = Mathf.Clamp(_whitePowerRead, 0, dangerSprites.Count - 1);
            dangerImage.sprite = dangerSprites[spriteIndex];
            text.text = $"Daggers Read: {_whitePowerRead.ToString()}";
        }
    }
}
