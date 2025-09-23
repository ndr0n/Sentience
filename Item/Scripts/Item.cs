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
        
    }
}