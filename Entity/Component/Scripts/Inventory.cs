using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

namespace Sentience
{
    [System.Serializable]
    public class Inventory : IEntityComponent
    {
        public int Size;
        public int Credits;
        public List<Slot> Items;

        [System.Serializable]
        public class Slot
        {
            [SerializeReference] public EntityData Data = null;
            public int Amount = 1;

            public Slot(Item item, int amount)
            {
                Data = item.Data;
                Amount = amount;
            }
        }

        EntityData _data;
        public EntityData Data => _data;

        public void Init(EntityData data, System.Random random)
        {
            _data = data;
        }

        public void Add(Item item)
        {
            // foreach (var existingItem in Items)
            // {
            // if (existingItem.Data.Name == item.Data.Name)
            // {
            // if (existingItem.Amount + item.Amount <= item.ItemType.Stack)
            // {
            // existingItem.Amount += item.Amount;
            // return;
            // }
            // break;
            // }
            // }
            Items.Add(new(item, 1));
        }

        public void Remove(Item item, int amount)
        {
            // if (item.Amount > amount) item.Amount -= amount;
            // else if (item.Amount == amount) Items.Remove(item);
            // else Debug.Log($"Not enough {item.Amount}.");
            foreach (var slot in Items)
            {
                if (slot.Data == item.Data)
                {
                    Items.Remove(slot);
                    break;
                }
            }
        }

        public void Remove(int index, int amount)
        {
            // if (Items[slot].Amount > amount) Items[slot].Amount -= amount;
            // else if (Items[slot].Amount == amount) Items.RemoveAt(slot);
            // else Debug.Log($"Not enough {Items[slot].Name}.");
            Items.RemoveAt(index);
        }

        public bool UseItem(string itemName)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Data.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase))
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

        public bool CanStore(EntityType itemType)
        {
            return Items.Count < Size;
        }
    }

    [System.Serializable]
    public class InventoryAuthoring : EntityComponentAuthoring
    {
        public int Size = 64;
        public int Credits = 0;
        public List<Inventory.Slot> Items = new();

        public override IEntityComponent Spawn(Random random)
        {
            Inventory inventory = new();
            inventory.Size = Size;
            inventory.Credits = Credits;
            inventory.Items = Items;
            return inventory;
        }
    }
}