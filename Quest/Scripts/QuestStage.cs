using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    [System.Serializable]
    public class QuestStage
    {
        public string Objective;
        public string Description;
        public InteractionData InteractionData;

        public async Awaitable InitFromSentienceQuestStage(SentienceQuestStage stage, Entity player, List<Entity> entities)
        {
            Objective = stage.Objective;
            Description = stage.Description;

            Entity? targetEntity = null;
            if (!string.IsNullOrEmpty(stage.Target))
            {
                List<string> entityNames = new();
                foreach (var d in entities)
                {
                    Info info = EntityLibrary.Get<Info>(d);
                    entityNames.Add($"{info.Name}|{info.Description}");
                }
                string eTarget = await SentienceManager.Instance.RagManager.GetMostSimilar(entityNames, stage.Target);
                eTarget = eTarget.Split('|')[0];
                targetEntity = entities.FirstOrDefault(x => EntityLibrary.Get<Info>(x).Name == eTarget);
            }
            if (targetEntity.HasValue == false)
            {
                targetEntity = entities[Random.Range(0, entities.Count)];
            }

            Entity target = targetEntity.Value;
            Entity owner = target;
            Info targetInfo = EntityLibrary.Get<Info>(target);

            string itemName = "";
            // string target = target.Name;
            if (EntityLibrary.Has<Item>(target))
            {
                Item item = EntityLibrary.Get<Item>(target);
                itemName = targetInfo.Name;
                foreach (var entity in entities)
                {
                    if (EntityLibrary.Has<Identity>(entity))
                    {
                        if (EntityLibrary.Has<Inventory>(entity))
                        {
                            Identity data = EntityLibrary.Get<Identity>(entity);
                            Inventory inv = EntityLibrary.Get<Inventory>(entity);
                            bool breakLoop = false;
                            foreach (var i in inv.Items)
                            {
                                if (item.Entity == i.Item.Entity)
                                {
                                    target = entity;
                                    owner = entity;
                                    breakLoop = true;
                                    break;
                                }
                            }
                            if (breakLoop) break;
                        }
                    }
                }
            }

            Interaction interaction = null;
            List<Interaction> eInteractions = targetInfo.Type.Interactions.Where(x => x.HasInteraction(target, player, owner)).ToList();
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
                interaction = eInteractions[Random.Range(0, targetInfo.Type.Interactions.Count)];
            }

            InteractionData = new(itemName, targetInfo.Name, interaction);
        }
    }
}