using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Store
{
    [CreateAssetMenu(fileName = "Store Database", menuName = "Content/Data/Store Database")]
    public class StoreDatabase : ScriptableObject
    {
        [SerializeField] TabData[] tabs;
        public TabData[] Tabs => tabs;

        [SerializeField] ProductData[] products;


    }

    [System.Serializable]
    public class TabData
    {
        [SerializeField] string uniqueId;
        public string UniqueId => uniqueId;

        [SerializeField] string name;
        public string Name => name;

        [SerializeField] TabType type;
        public TabType Type => type;
    }

}