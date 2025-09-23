using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "ItemData", menuName = "MindTheatre/ItemData")]
    public class ItemData : ScriptableObject
    {
        public List<Item> Items = new();

        public Item GetItem(string itemName)
        {
            return Items.FirstOrDefault(x => x.Name == itemName);
        }
    }
}