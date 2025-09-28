using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    public class Item : EntityComponent
    {
        public override void OnInit(EntityData data, System.Random random)
        {
        }
        
        public ItemType Type => (ItemType) Data.Type;
        
        public string Print()
        {
            string print = "";
            print += $"{Data.Name}\n";
            if (!string.IsNullOrWhiteSpace(Data.Description)) print += $"{Data.Description}\n";
            return print;
        }
    }
}