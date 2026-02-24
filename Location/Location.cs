using System.Collections.Generic;
using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    public class Location
    {
        public string Name = "";
        public string Description = "";
        public Vector3 Size = Vector3.one;
        public Vector3 Position = Vector3.zero;
        public Faction Faction;
        public List<EntityData> Objects = new();
        public List<EntityData> Characters = new();
    }
}