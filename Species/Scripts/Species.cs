using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Sentience/Species/Species")]
    public class Species : ScriptableObject
    {
        public string Name = "Species";
        public string Description = "";
        public List<EntityType> SpeciesTypes = new List<EntityType>();
    }
}