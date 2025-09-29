using MindTheatre;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace Sentience
{
    [System.Serializable]
    public class Identity : EntityComponent
    {
        public Species Species;
        public Faction Faction;
        public string Location;

        public async Awaitable LoadSentienceCharacter(SentienceCharacter character, Faction faction, Random random)
        {
            Data.Name = character.Name;
            Data.Description = character.Description;
            Faction = faction;
            Location = character.Location;
            Inventory inventory = Data.Get<Inventory>();
            if (inventory != null)
            {
                foreach (var item in character.Inventory)
                {
                    EntityData entityData = new(item, $"belongs to {character.Name}", await SentienceManager.Instance.RagManager.GetMostSimilarItem(SentienceManager.Instance.ItemDatabase, item), random);
                    Item itm = entityData.Get<Item>();
                    inventory.Add(itm);
                }
            }
        }
    }

    [System.Serializable]
    public class IdentityAuthoring : EntityComponentAuthoring
    {
        public Species Species;
        public Faction Faction;
        public string Location;

        public override IEntityComponent Spawn(Random random)
        {
            Identity identity = new Identity();
            identity.Species = Species;
            identity.Faction = Faction;
            identity.Location = Location;
            return identity;
        }
    }
}