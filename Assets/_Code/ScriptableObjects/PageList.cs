using System.Collections.Generic;
using UnityEngine;

namespace LSG.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PageList", menuName = "LSG/Make a Page List")]
    public class PageList : ScriptableObject
    {
        public List<CardData> Pages = new List<CardData>();
    }
}
