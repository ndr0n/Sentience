using System.Collections.Generic;
using Unity.Entities;
using Random = UnityEngine.Random;

namespace Sentience
{
    [System.Serializable]
    public class Persona : EntityComponent
    {
        public string Desire;

        public void RefreshDesire(List<EntityData> entities)
        {
            if (string.IsNullOrWhiteSpace(Desire))
            {
                List<EntityData> items = new();
                foreach (var entity in entities)
                {
                    if (entity.Has<Inventory>())
                    {
                        Inventory inv = entity.Get<Inventory>();
                        foreach (var slot in inv.Items) items.Add(slot.Item);
                    }
                }
                EntityData desiredItem = items[Random.Range(0, items.Count)];
                ID id = desiredItem.GetData<ID>();
                Desire = id.Name;
            }
        }
    }

    [System.Serializable]
    public class PersonaAuthoring : EntityAuthoring
    {
        public string Desire = "";

        public override IComponentData Spawn(System.Random random)
        {
            Persona persona = new();
            persona.Desire = Desire;
            return persona;
        }
    }
}