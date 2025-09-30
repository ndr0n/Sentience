using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    [System.Serializable]
    public class Item : EntityComponent
    {
        public int Price;
        public int Stack;
        public float Weight;
        public Sprite Icon;

        public string Print()
        {
            Info info = Data.Get<Info>();
            string print = "";
            print += $"{info.Name}\n";
            if (!string.IsNullOrWhiteSpace(info.Description)) print += $"{info.Description}\n";
            return print;
        }
    }

    [System.Serializable]
    public class ItemAuthoring : EntityAuthoring
    {
        public int Stack = 1;
        public Vector2 Weight = new Vector2(0, 1);
        public Vector2Int Price = new Vector2Int(1, 50);
        public List<Sprite> Icons = new();

        public override IEntityComponent Spawn(System.Random random)
        {
            Item item = new()
            {
                Stack = Stack,
                Price = random.Next(Price.x, Price.y),
                Weight = random.Next(Price.x, Price.y),
                Icon = Icons[random.Next(0, Icons.Count)]
            };
            return item;
        }
    }
}