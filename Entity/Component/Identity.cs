using System.Collections.Generic;
using System.Threading.Tasks;
using MindTheatre;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace Sentience
{
    [System.Serializable]
    public class Relationship
    {
        public Identity Identity;
        public Reputation Reputation;

        public Relationship(Identity identity, int reputation)
        {
            Identity = identity;
            Reputation = new Reputation(reputation);
        }
    }

    [System.Serializable]
    public class Identity : EntityComponent
    {
        public Species Species;
        public Faction Faction;
        public string Location = "";
        public string Description = "";
        public List<Relationship> Relationships = new();

        [SerializeField] float crimeLevel = 0;

        public float CrimeLevel
        {
            get => crimeLevel;
            set
            {
                crimeLevel = value;
                if (crimeLevel < 0) crimeLevel = 0;
                else if (crimeLevel > 100) crimeLevel = 100;
                OnUpdateCrimeLevel?.Invoke(crimeLevel);
            }
        }

        public System.Action<float> OnUpdateCrimeLevel = null;

        public async Task LoadSentienceCharacter(SentienceCharacter character, Faction faction, Random random)
        {
            ID id = Data.Get<ID>();
            Data.Name = character.Name;

            Spawn spawn = Data.Get<Spawn>();
            if (spawn != null && spawn.Spawned != null) spawn.Spawned.name = character.Name;

            id.Description = character.Description;
            Faction = faction;
            Location = character.Location;

            if (Data.Has<Inventory>())
            {
                Inventory inventory = Data.Get<Inventory>();
                foreach (var item in character.Inventory)
                {
                    EntityData itemData = new(item, $"belongs to {character.Name}", await SentienceManager.Instance.RagManager.GetMostSimilarItem(SentienceManager.Instance.ItemDatabase, item), random);
                    Item itm = itemData.Get<Item>();
                    inventory.Add(itm, 1);
                }
            }
        }

        public Relationship GetRelationship(Identity identity)
        {
            foreach (var rel in Relationships)
            {
                if (rel.Identity == identity) return rel;
            }

            Relationship relationship = new(identity, Faction.GetFactionReputation(identity.Faction).Value / 2);
            return relationship;
        }
    }

    [System.Serializable]
    public class IdentityAuthoring : EntityAuthoring
    {
        public Species Species;
        public Faction Faction;
        public string Location = "";
        public string Description = "";
        public Vector2Int CrimeLevel = new Vector2Int(0, 0);

        public override IEntityComponent Spawn(Random random)
        {
            Identity identity = new Identity();
            identity.Species = Species;
            identity.Faction = Faction;
            identity.Location = Location;
            identity.Description = Description;
            identity.CrimeLevel = random.Next(CrimeLevel.x, CrimeLevel.y + 1);
            return identity;
        }
    }
}