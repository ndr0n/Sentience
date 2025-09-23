using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Sentience/Faction/FactionData")]
    public class FactionData : ScriptableObject
    {
        public List<Faction> Faction = new();

        public void Init()
        {
            foreach (var f in Faction)
            {
                f.GenerateFactionRelationships(this);
            }
        }

        public int GetIndexFromFaction(Faction faction)
        {
            return Faction.IndexOf(faction);
        }

        public Faction GetFactionFromIndex(int index)
        {
            return Faction[index];
        }

        public Faction GetFaction(string factionName)
        {
            return Faction.FirstOrDefault(x => x.Name == factionName);
        }
    }
}