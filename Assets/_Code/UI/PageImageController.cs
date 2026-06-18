using UnityEngine;
using UnityEngine.UI;

namespace LSG
{
    public class PageImageController : MonoBehaviour
    {
        public Image TargetImage;

        public void SetImage(Sprite sprite)
        {
            TargetImage.sprite = sprite;
        }
        public void SetUnavailable(bool unavailable)
        {
            Color color = TargetImage.color;
            color.a = (unavailable ? 0.5f : 1f);
            TargetImage.color = color;
        }
    }
}
