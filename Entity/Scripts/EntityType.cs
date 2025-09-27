using System.Collections.Generic;
using UnityEngine;

namespace Sentience
{
    public class EntityType : ScriptableObject
    {
        public string Name = "Entity";
        public string Description = "";
        public List<Interaction> Interactions = new();
    }
}