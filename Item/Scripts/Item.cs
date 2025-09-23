using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Item", menuName = "Sentience/Item")]
    public class Item : ScriptableObject
    {
        public string Name = "";
        public string Description = "";
        public int Weight = 0;
        public int Price = 1;
        public int Amount = 1;
        public int Stack = 1;
        public Sprite Icon = null;

        public static Item Generate(string name, string description)
        {
            Item it = CreateInstance<Item>();
            it.Name = name;
            it.Description = description;
            it.Weight = 0;
            it.Price = 1;
            it.Amount = 1;
            it.Stack = 1;
            it.Icon = null;
            return it;
        }

        public int GetSellPrice()
        {
            return Price;
        }

        public int GetBuyPrice()
        {
            return Price;
        }

        public virtual bool Pickup(GameObject player)
        {
            return true;
        }

        public virtual bool UseItem(GameObject player)
        {
            return false;
        }

        public string Print()
        {
            string print = "";
            print += $"{Name}\n";
            if (!string.IsNullOrWhiteSpace(Description)) print += $"{Description}\n";
            return print;
        }
    }
}