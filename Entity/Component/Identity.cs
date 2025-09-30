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
            Info info = EntityLibrary.Get<Info>(Entity);
            info.Name = character.Name;
            info.Description = character.Description;
            // World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData<Info>(Entity, info);
            // EntityLibrary.Set<Info>(Entity, info);

            Faction = faction;
            Location = character.Location;

            if (EntityLibrary.Has<Inventory>(Entity))
            {
                Inventory inventory = EntityLibrary.Get<Inventory>(Entity);
                foreach (var item in character.Inventory)
                {
                    EntityInstance entityInstance = new(item, $"belongs to {character.Name}", await SentienceManager.Instance.RagManager.GetMostSimilarItem(SentienceManager.Instance.ItemDatabase, item), random);
                    Item itm = EntityLibrary.Get<Item>(Entity);
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

        public override IComponentData Spawn(Random random)
        {
            Identity identity = new Identity();
            identity.Species = Species;
            identity.Faction = Faction;
            identity.Location = Location;
            return identity;
        }
    }
}