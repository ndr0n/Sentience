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
            ID id = Data.Get<ID>();
            id.Name = character.Name;
            id.Description = character.Description;
            // Data.SetData(id);

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