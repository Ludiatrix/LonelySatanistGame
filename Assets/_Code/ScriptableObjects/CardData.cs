using LSG.Classes;
using LSG.Core;
using UnityEngine;

namespace LSG.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CardData", menuName = "LSG/Create a Page")]
    public class CardData : ScriptableObject
    {
        public int CardID = 0;
        public int TapeCost = 0;
        public string CardWord = "Beep";
        [TextArea] public string CardEffect = "";
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

        public AudioClip Sound;
        public ModifierPayload PageModifier;
    }
}
