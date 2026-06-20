using LSG.Core;
using UnityEngine;
using UnityEngine.UI;

namespace LSG
{
    /// <summary>
    /// Place on each store card (alongside its Button). Hooks the card's OnClick
    /// and toggles the shared Card Description panel for this card. Replaces the
    /// per-card EventTrigger Select/Deselect wiring.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class StoreCardClick : MonoBehaviour
    {
        [Tooltip("The shared description panel. Auto-found from parents if left empty.")]
        [SerializeField] private CardDescriptionPanel panel;

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();

            if (panel == null)
                panel = GetComponentInParent<CardDescriptionPanel>(true);

            if (panel == null)
                Debug.LogError($"{nameof(StoreCardClick)} on '{name}' could not find a CardDescriptionPanel.", this);
        }

        private void OnEnable()
        {
            button.onClick.AddListener(OnCardClicked);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(OnCardClicked);
        }

        private void OnCardClicked()
        {
            if (panel != null)
                panel.Toggle(GetComponent<StorePagePopulator>().cardData);
            
            UIEvents.StoreButtonClicked?.Invoke();
        }
    }
}
