using UnityEngine.Serialization;

namespace Sentience
{
    [System.Serializable]
    public struct FactionRelationship
    {
        public Faction Faction;
        public Reputation Reputation;

        public FactionRelationship(Faction faction, int value)
        {
            Faction = faction;
            Reputation = new Reputation(value);
        }
    }
}