using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    public class Item : Component
    {
        public ItemType Type => (ItemType) Data.Type;

        public Item(EntityData data) : base(data)
        {
            _data = data;
        }

        public string Print()
        {
            string print = "";
            print += $"{Data.Name}\n";
            if (!string.IsNullOrWhiteSpace(Data.Description)) print += $"{Data.Description}\n";
            return print;
        }
    }
}