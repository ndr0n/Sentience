using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Item", menuName = "Sentience/Item")]
    public class ItemType : ScriptableObject
    {
        public string Name = "Item";
        public string Description = "Description";
        public int Weight = 0;
        public int Price = 1;
        public int Stack = 1;
        public Sprite Icon = null;

        public static async Awaitable<ItemType> GetType(ItemData itemData, string itemName)
        {
            return await SentienceManager.Instance.RagManager.GetMostSimilarItem(itemData, itemName);
        }

        public int GetSellPrice()
        {
            return Price;
        }

        public int GetBuyPrice()
        {
            return Price;
        }

        public virtual bool Pickup(Identity player, Item item)
        {
            return true;
        }

        public virtual bool UseItem(Identity player, Item item)
        {
            return false;
        }
    }
}