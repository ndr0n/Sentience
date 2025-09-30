using System.Collections.Generic;
using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    public class Location
    {
        public string Name = "";
        public string Description = "";
        public Faction Faction;
        public Vector3 Size = Vector3.one;
        public Vector3 Position = Vector3.zero;
        public List<EntityInstance> Objects = new();
        public List<EntityInstance> Characters = new();
    }
}