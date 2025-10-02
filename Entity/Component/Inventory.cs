using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

namespace Sentience
{
    [System.Serializable]
    public class Inventory : EntityComponent
    {
        public int Size;
        public int Credits;
        public List<Slot> Items;

        [System.Serializable]
        public struct Slot : IEquatable<Slot>
        {
            [HideInInspector] public string Name;
            public int Amount;
            [SerializeReference] public EntityData Item;

            public Slot(Item item, int amount)
            {
                ID id = item.Data.Get<ID>();
                Name = id.Name;
                Item = item.Data;
                Amount = amount;
            }

            public bool Equals(Slot other)
            {
                return Item == other.Item;
            }
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
                if (slot.Item == item.Data)
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
                ID id = Items[i].Item.Get<ID>();
                if (id.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase))
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
    public class InventoryAuthoring : EntityAuthoring
    {
        public int Size = 64;
        public Vector2Int Credits = new Vector2Int(0, 100);
        public List<EntityType> Items = new();

        public override IEntityComponent Spawn(Random random)
        {
            Inventory inventory = new();
            inventory.Size = Size;
            inventory.Credits = random.Next(Credits.x, Credits.y);
            inventory.Items = new();
            foreach (var itm in Items)
            {
                EntityData itemData = new(itm.Name, itm.Description, itm, random);
                Item item = itemData.Get<Item>();
                inventory.Items.Add(new(item, 1));
            }
            return inventory;
        }
    }
}