using LSG.Core;
using UnityEngine;

namespace LSG.UI
{
    public class ResourceUIController : MonoBehaviour
    {
        private static readonly int BrightnessId = Shader.PropertyToID("_Brightness");

        public CanvasGroup group;
        public SpriteRenderer flameRenderer;

        void OnEnable()
        {
            UIEvents.ToggleResourceUI?.AddListener(OnToggleResourceUI);
        }

        private void OnToggleResourceUI(bool toggle)
        {
            float setting = toggle ? 1f : 0f;

            group.alpha = setting;

            if (flameRenderer != null)
            {
                flameRenderer.material.SetFloat(BrightnessId, setting);
            }

        }
    }
}
