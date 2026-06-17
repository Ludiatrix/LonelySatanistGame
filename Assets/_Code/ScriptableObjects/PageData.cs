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
        public ModifierPayload PageModifier;
    }
}
