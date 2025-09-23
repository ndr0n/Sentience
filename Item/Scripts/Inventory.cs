using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sentience
{
    public class Inventory
    {
        public int Size = 64;
        public int Credits = 50;
        public List<Item> Items = new();

        public void Add(Item item)
        {
            foreach (var existingItem in Items)
            {
                if (existingItem == item)
                {
                    if (existingItem.Amount + item.Amount <= item.Stack)
                    {
                        existingItem.Amount += item.Amount;
                        return;
                    }
                    break;
                }
            }
            Items.Add(item);
        }

        public void Remove(int slot, int amount)
        {
            if (Items[slot].Amount > amount) Items[slot].Amount -= amount;
            else if (Items[slot].Amount == amount) Items.RemoveAt(slot);
            else Debug.Log($"Not enough {Items[slot].name}.");
        }

        public bool UseItem(string itemName)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Name.Equals(itemName, StringComparison.OrdinalIgnoreCase))
                {
                    Remove(i, 1);
                    return true;
                }
            }
            return false;
        }

        public bool CanPayFor(int cost)
        {
            if (Credits >= cost) return true;
            else return false;
        }

        public bool CanStore(Item item)
        {
            return Items.Count < Size;
        }
    }
}