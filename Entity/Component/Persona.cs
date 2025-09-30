using System;
using System.Collections.Generic;
using System.Linq;
using MindTheatre;
using Sentience;
using Unity.Entities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Sentience
{
    [System.Serializable]
    public class Persona : EntityComponent
    {
        public string Desire;

        public void RefreshDesire(List<Entity> entities)
        {
            if (string.IsNullOrWhiteSpace(Desire))
            {
                List<Item> items = new();
                foreach (var entity in entities)
                {
                    if (EntityLibrary.Has<Inventory>(entity))
                    {
                        Inventory inv = EntityLibrary.Get<Inventory>(entity);
                        foreach (var slot in inv.Items) items.Add(slot.Item);
                    }
                }
                Item desiredItem = items[Random.Range(0, items.Count)];
                Info info = EntityLibrary.Get<Info>(desiredItem.Entity);
                Desire = info.Name;
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