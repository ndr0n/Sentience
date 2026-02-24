using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Sentience
{
    [System.Serializable]
    public class QuestStage
    {
        public string Objective;
        public string Description;
        public InteractionData InteractionData;

        public async Awaitable InitFromSentienceQuestStage(SentienceQuestStage stage, EntityData player, List<EntityData> entities)
        {
            Objective = stage.Objective;
            Description = stage.Description;
            EntityData targetData = null;
            if (!string.IsNullOrEmpty(stage.Target))
            {
                List<string> entityNames = new();
                foreach (var d in entities)
                {
                    ID entityID = d.Get<ID>();
                    entityNames.Add($"{d.Name}|{entityID.Description}");
                }
                string eTarget = await SentienceManager.Instance.RagManager.GetMostSimilar(entityNames, stage.Target);
                eTarget = eTarget.Split('|')[0];
                targetData = entities.FirstOrDefault(x => x.Name == eTarget);
            }
            if (targetData == null)
            {
                targetData = entities[Random.Range(0, entities.Count)];
            }

            string itemName = "";
            string targetName = targetData.Name;
            EntityData itemOwner = null;
            if (targetData.Has<Item>())
            {
                Item item = targetData.Get<Item>();
                itemName = item.Data.Name;
                foreach (var entity in entities)
                {
                    if (entity.Has<Inventory>())
                    {
                        Inventory inv = entity.Get<Inventory>();
                        bool breakLoop = false;
                        foreach (var i in inv.Items)
                        {
                            if (item.Data == i.Item)
                            {
                                targetName = entity.Name;
                                itemOwner = entity;
                                breakLoop = true;
                                break;
                            }
                        }
                        if (breakLoop) break;
                    }
                }
            }

            Interaction interaction = null;
            ID targetID = targetData.Get<ID>();
            List<Interaction> eInteractions = targetID.Type.Interactions.Where(x => x.HasInteraction(targetData, player, itemOwner)).ToList();
            if (!string.IsNullOrWhiteSpace(stage.Action))
            {
                List<string> interactions = new();
                foreach (var inter in eInteractions) interactions.Add($"{inter.Name}|{inter.Description}|{inter.Tags}");
                string eInteractionName = await SentienceManager.Instance.RagManager.GetMostSimilar(interactions, stage.Action);
                eInteractionName = eInteractionName.Split('|')[0];
                interaction = eInteractions.FirstOrDefault(x => x.Name == eInteractionName);
            }
            if (interaction == null)
            {
                interaction = eInteractions[Random.Range(0, targetID.Type.Interactions.Count)];
            }
            InteractionData = new(itemName, targetName, interaction);
        }
    }
}