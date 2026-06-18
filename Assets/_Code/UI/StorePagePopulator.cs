using System.Collections.Generic;
using LSG.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace LSG
{
    public class StorePagePopulator : MonoBehaviour
    {
        public PageImageController pageImageController;
        public List<Image> tapeImages = new();

        public void SetPageData(PageData data, bool owned)
        {
            if (!owned)
            {
                pageImageController.SetImage(data.PageImageRipped);
                for (var i = 0; i < tapeImages.Count; i++)
                {
                    tapeImages[i].gameObject.SetActive(i < data.PageModifier.Tape);
                }
            } else
            {
                pageImageController.SetImage(data.PageImage);
                pageImageController.SetUnavailable(true);
                for (var i = 0; i < tapeImages.Count; i++)
                {
                    tapeImages[i].gameObject.SetActive(false);
                }

            }
        }
    }
}
