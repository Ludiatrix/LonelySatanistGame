using UnityEngine;

namespace LSG.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CardList", menuName = "LSG/Make a CardList")]
    public class CardList : ScriptableObject
    {
        public CardData[] Cards;
    }
}
