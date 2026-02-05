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

        public override void OnInit(EntityData data, Random random)
        {
            base.OnInit(data, random);
            foreach (var item in Items) item.Item.Data.Init(random);
        }

        [System.Serializable]
        public class Slot
        {
            public int Amount;
            [SerializeReference] public Item Item;

            public Slot(Item item, int amount)
            {
                Item = item;
                Amount = amount;
            }
        }

        public void Add(Item item, int amount)
        {
            foreach (var existingItem in Items)
            {
                if (existingItem.Item.Data.Name == item.Data.Name)
                {
                    if (existingItem.Amount + amount <= item.Stack)
                    {
                        existingItem.Amount += amount;
                        return;
                    }
                }
            }

            Items.Add(new(item, amount));
        }

        public void Remove(Slot slot, int amount)
        {
            slot.Amount -= amount;
            if (slot.Amount <= 0) Items.Remove(slot);
        }

        public void Remove(Item item, int amount)
        {
            Slot s = null;
            foreach (var slot in Items)
            {
                if (slot.Item == item)
                {
                    s = slot;
                }
            }

            if (s != null)
            {
                s.Amount -= amount;
                if (s.Amount <= 0) Items.Remove(s);
            }
        }

        public bool UseItem(string itemName)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                ID id = Items[i].Item.Data.ID;
                if (id.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase))
                {
                    Remove(Items[i], 1);
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

        public bool CanStore(Item item, int amount)
        {
            foreach (var existingItem in Items)
            {
                if (existingItem.Item.Data.Name == item.Data.Name)
                {
                    if (existingItem.Amount + amount <= item.Stack)
                    {
                        return true;
                    }
                }
            }

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