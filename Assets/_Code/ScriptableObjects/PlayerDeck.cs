using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LSG.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayerDeck", menuName = "LSG/Make a Deck")]
    public class PlayerDeck : ScriptableObject
    {
        [SerializeField] private List<PageData> _pages = new List<PageData>();
        private readonly List<PageData> _usedPages = new List<PageData>();

        public List<PageData> UsedPages => _usedPages;

        public void AddPage(PageData page)
        {
            _pages.Add(page);
            _pages.Shuffle();
        }

        public PageData TakePage()
        {
            if (_pages.Count == 0)
            {
                Debug.Log($"[PlayerDeck] There are no more pages... how did you do that?");
                return null;
            }
            PageData page = _pages[_pages.Count - 1]; // Riza: fun fact getting the end index is faster than the first
            _usedPages.Add(page);
            _pages.RemoveAt(_pages.Count - 1);
            return page;
        }
        
        public void Shuffle()
        {
            _pages.Shuffle();
        }

        public void SetToDefault()
        {
            var defaultDeck = Resources.Load<PlayerDeck>($"DefaultDeck");
            _pages = defaultDeck._pages;
            Shuffle();
        }
    }
}
