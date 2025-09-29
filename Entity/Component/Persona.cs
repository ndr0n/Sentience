using System;
using System.Collections.Generic;
using System.Linq;
using MindTheatre;
using Sentience;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Sentience
{
    [System.Serializable]
    public class Persona : IEntityComponent
    {
        EntityData _data;
        public EntityData Data => _data;

        public void Init(EntityData data, System.Random random)
        {
            _data = data;
        }

        public Identity Identity => Data.Get<Identity>();
        public string Desire = "";

        public void RefreshDesire(List<EntityData> entities)
        {
            if (string.IsNullOrWhiteSpace(Desire))
            {
                List<Item> items = new();
                foreach (var entity in entities)
                {
                    Inventory inv = entity.Get<Inventory>();
                    if (inv == null) continue;
                    foreach (var slot in inv.Items) items.Add(slot.Item);
                }
                Item desiredItem = items[Random.Range(0, items.Count)];
                Desire = desiredItem.Data.Name;
            }
        }
    }

    [System.Serializable]
    public class PersonaAuthoring : EntityComponentAuthoring
    {
        public string Desire = "";

        public override IEntityComponent Spawn(System.Random random)
        {
            Persona persona = new();
            persona.Desire = Desire;
            return persona;
        }
    }
}