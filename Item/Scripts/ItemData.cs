using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "ItemData", menuName = "Sentience/Item/Data")]
    public class ItemData : ScriptableObject
    {
        public List<ItemType> Items = new();

        public ItemType GetItem(string itemName)
        {
            return Items.FirstOrDefault(x => x.Name == itemName);
        }
    }
}