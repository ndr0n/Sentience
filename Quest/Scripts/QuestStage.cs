using System.Collections.Generic;
using System.Linq;
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

        public async Awaitable InitFromSentienceQuestStage(SentienceQuestStage stage, EntityData player, List<EntityData> entities)
        {
            Objective = stage.Objective;
            Description = stage.Description;

            EntityData target = null;
            if (!string.IsNullOrEmpty(stage.Target))
            {
                List<string> entityNames = new();
                foreach (var entity in entities)
                {
                    Info info = entity.Get<Info>();
                    entityNames.Add($"{info.Name}|{info.Description}");
                }
                string eTarget = await SentienceManager.Instance.RagManager.GetMostSimilar(entityNames, stage.Target);
                eTarget = eTarget.Split('|')[0];
                target = entities.FirstOrDefault(x => x.Name == eTarget);
            }
            if (target == null)
            {
                target = entities[Random.Range(0, entities.Count)];
            }

            EntityData owner = target;
            Info targetInfo = target.Get<Info>();

            string itemName = "";
            // string target = target.Name;
            if (target.Has<Item>())
            {
                Item itemTarget = target.Get<Item>();
                itemName = targetInfo.Name;
                foreach (var entity in entities)
                {
                    if (entity.Has<Identity>())
                    {
                        if (entity.Has<Inventory>())
                        {
                            Identity identity = entity.Get<Identity>();
                            Inventory inv = entity.Get<Inventory>();
                            bool breakLoop = false;
                            foreach (var i in inv.Items)
                            {
                                if (itemTarget.Data == i.Item.Data)
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