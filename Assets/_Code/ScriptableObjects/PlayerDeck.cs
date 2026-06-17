using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LSG.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayerDeck", menuName = "LSG/Make a Deck")]
    public class PlayerDeck : ScriptableObject
    {
        private List<PageData> _pages = new List<PageData>();

        public void AddPage(PageData page)
        {
            _pages.Add(page);
            _pages.Shuffle();
        }

        public PageData TakePage()
        {
            PageData page = _pages[_pages.Count - 1]; // Riza: fun fact getting the end index is faster than the first
            _pages.RemoveAt(_pages.Count - 1);
            return page;
        }
        
        public void Shuffle()
        {
            _pages.Shuffle();
        }
    }
}
