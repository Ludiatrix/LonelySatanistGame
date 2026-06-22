using LSG.Classes;
using LSG.Core;
using LSG.Effects;
using UnityEngine;
using static LSG.Core.Enums;

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
		public Sprite SummoningPage;
		//this is the store image. i didnt change the name becuase i didnt want to deal with it <3
        public Sprite PageImage;

        [SerializeField] private Sprite pageImageRipped;

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

        [Header("Effects")]
        public CardEffectType EffectType;
    }
}