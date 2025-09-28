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

            EntityData entityDataTarget = null;
            if (!string.IsNullOrEmpty(stage.Target))
            {
                List<string> entityNames = new();
                foreach (var d in entities) entityNames.Add($"{d.Name}|{d.Description}");
                string eTarget = await SentienceManager.Instance.RagManager.GetMostSimilar(entityNames, stage.Target);
                eTarget = eTarget.Split('|')[0];
                entityDataTarget = entities.FirstOrDefault(x => x.Name == eTarget);
            }
            if (entityDataTarget == null)
            {
                entityDataTarget = entities[Random.Range(0, entities.Count)];
            }

            string itemName = "";
            string target = entityDataTarget.Name;
            EntityData itemOwner = null;
            Item item = entityDataTarget.Get<Item>();
            if (item != null)
            {
                itemName = item.Data.Name;
                foreach (var entity in entities)
                {
                    Identity data = entity.Get<Identity>();
                    if (data != null)
                    {
                        Inventory inv = entity.Get<Inventory>();
                        if (inv != null)
                        {
                            bool breakLoop = false;
                            foreach (var i in inv.Items)
                            {
                                if (item.Data == i.Item.Data)
                                {
                                    target = entity.Name;
                                    itemOwner = entity;
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
            List<Interaction> eInteractions = entityDataTarget.Type.Interactions.Where(x => x.HasInteraction(entityDataTarget, player, itemOwner)).ToList();
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
                interaction = eInteractions[Random.Range(0, entityDataTarget.Type.Interactions.Count)];
            }

            InteractionData = new(itemName, target, interaction);
        }
    }
}