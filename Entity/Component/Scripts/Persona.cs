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
    public class Persona : EntityComponent
    {
        public string Species = "";
        public string Desire = "";

        public override void OnInit(EntityData data, System.Random random)
        {
        }
        
        public ID ID => Data.Get<ID>();
        
        public async Awaitable Init(SentienceCharacter character)
        {
            Data.Name = character.Name;
            ID.Location = character.Location;
            Data.Description = character.Description;
            ID.Faction = character.Faction;
            Species = character.Species;
            Inventory inventory = Data.Get<Inventory>();
            if (inventory != null)
            {
                foreach (var item in character.Inventory)
                {
                    EntityData entityData = new(item, $"Belongs to {character.Name}", await SentienceManager.Instance.RagManager.GetMostSimilarItem(SentienceManager.Instance.ItemDatabase, item), new(Random.Range(int.MinValue, int.MaxValue)));
                    Item itm = entityData.Get<Item>();
                    inventory.Add(itm);
                }
            }
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