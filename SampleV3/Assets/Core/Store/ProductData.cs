using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Store
{
    [System.Serializable]
    public class ProductData
    {
        [SerializeField] string uniqueId;
        public string UniqueId => uniqueId;

        // Editor
        [SerializeField] bool isExpanded;

        [Space]
        [SerializeField] int id;
        [SerializeField] int tabId;

        public int Id => id;
        public int TabId => tabId;
        public TabType Tab => (TabType)tabId;

        [Space]
        [SerializeField] string name;
        public string Name => name;

        [Space]
        [SerializeField] PurchaseType purchaseType;
        [SerializeField] PriceData[] prices;

        public PurchaseType PurchType => purchaseType;
        public PriceData[] Prices => prices;

        public enum PurchaseType
        {
            InGameCurrency = 0,
            RewardedVideo = 1,
            IAP = 2,
        }
    }

    [System.Serializable]
    public class PriceData
    {
        [SerializeField] CurrencyType currency;
        [SerializeField] int cost = 1;
        public CurrencyType Currency => currency;
        public int Cost => cost;
    }
}