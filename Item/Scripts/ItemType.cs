using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Item", menuName = "Sentience/Item/Type")]
    public class ItemType : ScriptableObject
    {
        public string Name = "Item";
        public string Description = "Description";
        public int Weight = 0;
        public int Price = 1;
        public int Stack = 1;
        public Sprite Icon = null;
        public List<ItemInteraction> Interactions = new();

        public int GetSellPrice()
        {
            return Price;
        }

        public int GetBuyPrice()
        {
            return Price;
        }

        public static async Awaitable<ItemType> GetType(ItemData itemData, string itemName)
        {
            return await SentienceManager.Instance.RagManager.GetMostSimilarItem(itemData, itemName);
        }

        public virtual bool Interact(Item item, IdentityData owner, IdentityData interactor, ItemInteraction interaction)
        {
            return interaction.TryExecute(item, owner, interactor);
        }
    }
}