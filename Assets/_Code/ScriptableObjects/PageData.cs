using LSG.Classes;
using LSG.Core;
using UnityEngine;

namespace LSG.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PageData", menuName = "LSG/Create a Page")]
    public class PageData : ScriptableObject
    {
        public Enums.Suit Suit = Enums.Suit.White;
        public Sprite PageImage;
        [SerializeField]
        private Sprite pageImageRipped;
        public Sprite PageImageRipped
        {
            get
            {
                if (pageImageRipped == null) return PageImage;
                return pageImageRipped;
            }
        }
        public ModifierPayload PageModifier;
    }
}
