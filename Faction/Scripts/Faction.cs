using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    public enum FactionLaw
    {
        Unlawful,
        Lawful,
        Enforcer,
    }

    [System.Serializable]
    [CreateAssetMenu(menuName = "Sentience/Faction/Faction")]
    public class Faction : ScriptableObject
    {
        public string Name = "Faction";
        public string Description = "";
        public Sprite Icon = null;
        public Color Color = Color.white;
        public FactionLaw Law = FactionLaw.Unlawful;
        public List<EntityType> FactionEntity = new();
        public List<FactionRelationship> FactionRelationship = new();

        public void GenerateFactionRelationships(FactionDatabase factionDatabase)
        {
            foreach (var faction in factionDatabase.Faction)
            {
                if (FactionRelationship.Exists(x => x.Faction == faction)) continue;

                int reputation = 0;
                if (faction == this)
                {
                    reputation = 100;
                }
                else
                {
                    switch (Law)
                    {
                        case FactionLaw.Unlawful:
                            reputation = 0;
                            break;
                        case FactionLaw.Lawful:
                            switch (faction.Law)
                            {
                                case FactionLaw.Unlawful:
                                    reputation = 0;
                                    break;
                                case FactionLaw.Lawful:
                                    reputation = 80;
                                    break;
                                case FactionLaw.Enforcer:
                                    reputation = 60;
                                    break;
                            }

                            break;
                        case FactionLaw.Enforcer:
                            switch (faction.Law)
                            {
                                case FactionLaw.Unlawful:
                                    reputation = 0;
                                    break;
                                case FactionLaw.Lawful:
                                    reputation = 60;
                                    break;
                                case FactionLaw.Enforcer:
                                    reputation = 80;
                                    break;
                            }

                            break;
                    }
                }

                FactionRelationship fr = new(faction, reputation);
                FactionRelationship.Add(fr);
            }
        }

        public Reputation GetFactionReputation(Faction faction)
        {
            return FactionRelationship.First(x => x.Faction == faction).Reputation;
        }

        public bool IsHostile(Identity identity)
        {
            Reputation r = GetFactionReputation(identity.Faction);
            if (r.Sentiment == Sentiment.Hated) return true;

            switch (Law)
            {
                case FactionLaw.Unlawful:
                    return false;
                case FactionLaw.Lawful:
                    switch (identity.Faction.Law)
                    {
                        case FactionLaw.Unlawful:
                            return false;
                        case FactionLaw.Lawful:
                            if (identity.CrimeLevel >= 60) return true;
                            return false;
                        case FactionLaw.Enforcer:
                            return false;
                    }

                    break;
                case FactionLaw.Enforcer:
                    switch (identity.Faction.Law)
                    {
                        case FactionLaw.Unlawful:
                            return false;
                        case FactionLaw.Lawful:
                            if (identity.CrimeLevel >= 20) return true;
                            return false;
                        case FactionLaw.Enforcer:
                            return false;
                    }

                    break;
            }

            return false;
        }
    }
}