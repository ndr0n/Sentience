using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    [System.Serializable]
    public class Item : IEntityComponent
    {
        EntityData _data;
        public EntityData Data => _data;

        public void Init(EntityData data, System.Random random)
        {
            _data = data;
        }

        public int Price;
        public int Stack;
        public float Weight;
        public Sprite Icon;

        public string Print()
        {
            string print = "";
            print += $"{Data.Name}\n";
            if (!string.IsNullOrWhiteSpace(Data.Description)) print += $"{Data.Description}\n";
            return print;
        }
    }

    [System.Serializable]
    public class ItemAuthoring : EntityComponentAuthoring
    {
        public int Stack = 1;
        public Vector2 Weight = new Vector2(0, 1);
        public Vector2Int Price = new Vector2Int(1, 50);
        public List<Sprite> Icons = new();

        public override IEntityComponent Spawn(System.Random random)
        {
            Item item = new();
            item.Stack = Stack;
            item.Price = random.Next(Price.x, Price.y);
            item.Weight = random.Next(Price.x, Price.y);
            item.Icon = Icons[random.Next(0, Icons.Count)];
            return item;
        }
    }
}