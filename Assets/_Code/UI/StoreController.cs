using System;
using System.Collections.Generic;
using LSG.ScriptableObjects;
using UnityEngine;

namespace LSG
{
    public class StoreController : MonoBehaviour
    {
        public List<StorePagePopulator> storePages = new();

        public StorePagePopulator previewPage;

        // TODO: replace list of bools with some resource reference to look up ownership status
        public void SetPurchaseablePages(PageList pageList, List<bool> ownedIndices)
        {
            for (var i = 0; i < Math.Min(storePages.Count, pageList.Pages.Count); i++)
            {
                bool owned = false;
                if (ownedIndices.Count >= (i + 1) ) owned = ownedIndices[i];
                storePages[i].SetPageData(pageList.Pages[i], owned);
            }
        }
    }
}
