using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Sentience/Faction/Faction")]
    public class Faction : ScriptableObject
    {
        public string Name = "Faction";
        public string Description = "";
        public Sprite Icon = null;
        public Color Color = Color.white;
        public bool IsLawfulFaction = true;
        public List<FactionRelationship> FactionRelationship = new();
        public List<IdentityType> FactionIdentity = new();

        public virtual void GenerateFactionRelationships(FactionData factionData)
        {
            foreach (var faction in factionData.Faction)
            {
                if (FactionRelationship.All(x => x.Faction != faction))
                {
                    int startingValue = 0;
                    if (faction == this) startingValue = 100;
                    FactionRelationship fr = new(faction, startingValue);
                    FactionRelationship.Add(fr);
                }
            }
        }

        public virtual Reputation GetFactionReputation(Faction faction)
        {
            return FactionRelationship.First(x => x.Faction == faction).Reputation;
        }

        public virtual bool IsHostile(Faction faction)
        {
            Reputation r = GetFactionReputation(faction);
            if (r.Sentiment == Sentiment.Hated) return true;
            return false;
        }
    }
}