using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace LSG
{
    /// <summary>
    /// Adds OnSelect / OnDeselect events to a button. The regular Unity Button
    /// only exposes OnClick, but the EventSystem still fires selection callbacks —
    /// this surfaces them as inspector UnityEvents and toggles a target object.
    /// Place this alongside the Button component (it works on any Selectable).
    /// </summary>
    public class StoreButton : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [Header("Target")]
        [Tooltip("Object enabled when this button is selected, disabled when deselected.")]
        public GameObject targetGameObject;

        [Header("Events (optional extra hooks)")]
        public UnityEvent onSelect;
        public UnityEvent onDeselect;

        public void OnSelect(BaseEventData eventData)
        {
            if (targetGameObject != null)
                targetGameObject.SetActive(true);

            onSelect.Invoke();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            if (targetGameObject != null)
                targetGameObject.SetActive(false);

            onDeselect.Invoke();
        }
    }
}
