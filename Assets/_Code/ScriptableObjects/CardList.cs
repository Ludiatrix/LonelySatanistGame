using UnityEngine;
using UnityEngine.Serialization;

namespace LSG.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CardList", menuName = "LSG/Make a CardList")]
    public class CardList : ScriptableObject
    {
        [SerializeField]
        [FormerlySerializedAs("Cards")]
        private CardData[] cards;

        public CardData[] Cards => (CardData[]) cards.Clone();
    }
}
