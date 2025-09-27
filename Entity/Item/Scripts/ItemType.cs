using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Item", menuName = "Sentience/Item/Type")]
    public class ItemType : EntityType
    {
        public int Weight = 0;
        public int Price = 1;
        public int Stack = 1;
        public Sprite Icon = null;
        
        public int GetSellPrice()
        {
            return Price;
        }

        public int GetBuyPrice()
        {
            return Price;
        }

        public static async Awaitable<ItemType> GetType(ItemDatabase itemDatabase, string itemName)
        {
            return await SentienceManager.Instance.RagManager.GetMostSimilarItem(itemDatabase, itemName);
        }

        public virtual bool TryInteract(Item item, IdentityData interactor, IdentityData target, ItemInteraction interaction)
        {
            if (interaction.TryInteract(item, interactor, target))
            {
                Debug.Log($"Interacted: {interaction.name}");
                return true;
            }
            Debug.Log($"Failed Interaction: {interaction.name}");
            return false;
        }
    }
}