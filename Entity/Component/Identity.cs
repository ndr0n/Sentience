using MindTheatre;
using Unity.Entities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;
using Random = System.Random;
using World = Unity.Entities.World;

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
            Info info = Data.Get<Info>();
            info.Name = character.Name;
            info.Description = character.Description;

            Faction = faction;
            Location = character.Location;

            if (Data.Has<Inventory>())
            {
                Inventory inventory = Data.Get<Inventory>();
                foreach (var item in character.Inventory)
                {
                    EntityData data = new(item, $"belongs to {character.Name}", await SentienceManager.Instance.RagManager.GetMostSimilarItem(SentienceManager.Instance.ItemDatabase, item), random);
                    Item itm = Data.Get<Item>();
                    inventory.Add(itm);
                }
            }
        }
    }

    [System.Serializable]
    public class IdentityAuthoring : EntityAuthoring
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