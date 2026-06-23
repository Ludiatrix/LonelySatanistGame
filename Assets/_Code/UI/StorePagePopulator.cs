using System.Collections.Generic;
using LSG.ScriptableObjects;
using LSG.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace LSG
{
    public class StorePagePopulator : MonoBehaviour
    {
        public Vector3 growAmount;
        public PageImageController pageImageController;
        public Color successColor;
        public Color failColor;
        public List<Image> tapeImages = new();
        public CardData cardData;
        public SmoothGrower grower;
        
        public void SetPageDataToBuy (CardData data) => SetPageData(data, false);
        
        public void SetPageData(CardData data, bool owned)
        {
            cardData = data;
            if (!owned)
            {
                pageImageController.SetImage(data.PageImageRipped);
                for (var i = 0; i < tapeImages.Count; i++)
                {
                    tapeImages[i].gameObject.SetActive(i < data.TapeCost);
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

            if (gameObject.TryGetComponent(out Button btn))
            {
                btn.interactable = !owned;
            }
        }

        public void SuccessPulse()
        {
            pageImageController.SetColor(successColor);
            Pulse();
        }

        public void FailPulse()
        {
            pageImageController.SetColor(failColor);
            Pulse();
        }

        public void Pulse()
        {
            grower.GrowToTarget(growAmount,1.0f);
        }
    }
}
