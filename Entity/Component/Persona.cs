using System;
using System.Collections.Generic;
using System.Linq;
using DND;
using MindTheatre;
using Sentience;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Sentience
{
    [System.Serializable]
    public class Persona : Component
    {
        public string Species = "";
        public string Desire = "";

        public ID ID => Data.Get<ID>();

        public Persona(EntityData data) : base(data)
        {
            _data = data;
        }

        public static async Awaitable<Persona> Generate(ID id, SentienceCharacter character)
        {
            Persona persona = new(id.Data);
            id.Data.Name = character.Name;
            id.Location = character.Location;
            id.Data.Description = character.Description;
            id.Faction = character.Faction;
            persona.Species = character.Species;
            Inventory inventory = id.Data.Get<Inventory>();
            if (inventory != null)
            {
                foreach (var item in character.Inventory)
                {
                    EntityData entityData = new(item, $"Belongs to {character.Name}", await SentienceManager.Instance.RagManager.GetMostSimilarItem(SentienceManager.Instance.ItemDatabase, item), new(), new(Random.Range(int.MinValue, int.MaxValue)));
                    Item itm = entityData.Get<Item>();
                    inventory.Add(itm);
                }
            }
            return persona;
        }

        public void RefreshDesire(List<EntityData> entities)
        {
            if (string.IsNullOrWhiteSpace(Desire))
            {
                List<Item> items = new();
                foreach (var entity in entities)
                {
                    Inventory inv = entity.Get<Inventory>();
                    if (inv == null) continue;
                    items.AddRange(inv.Items);
                }
                Item desiredItem = items[Random.Range(0, items.Count)];
                Desire = desiredItem.Data.Name;
            }
        }
    }
}