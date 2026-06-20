using System.Collections.Generic;
using LSG.ScriptableObjects;
using LSG.UI;
using UnityEngine;

namespace LSG
{
    public class StoreTestRunner : MonoBehaviour
    {
        [Header("Core config")]
        
        public StoreController storeController;

        [Header("Test data")]
        public PageList StorePageList;
        public List<bool> OwnedPageIndices = new();

        void Start()
        {
            storeController.SetPurchaseablePages(StorePageList, OwnedPageIndices);
        }

    }
}
