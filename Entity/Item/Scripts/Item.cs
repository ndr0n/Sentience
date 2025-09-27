using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    public class Item : EntityData
    {
        public int Amount;
        public ItemType ItemType => (ItemType) Type;

        public Item(string name, string description, int amount, ItemType itemType, IdentityData owner)
        {
            Name = name;
            Description = description;
            Amount = amount;
            Type = itemType;
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