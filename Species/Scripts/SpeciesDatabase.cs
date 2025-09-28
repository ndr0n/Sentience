using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Sentience/Species/SpeciesDatabase")]
    public class SpeciesDatabase : ScriptableObject
    {
        public List<Species> Species = new();

        public int GetIndexFromSpecies(Species species)
        {
            return Species.IndexOf(species);
        }

        public Species GetFactionFromIndex(int index)
        {
            return Species[index];
        }

        public Species GetFaction(string speciesName)
        {
            return Species.FirstOrDefault(x => x.Name == speciesName);
        }
    }
}