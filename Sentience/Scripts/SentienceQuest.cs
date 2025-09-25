using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    public enum SentienceQuestAction
    {
        Speak,
        // Chat,
        // Talk,
        // Approach,

        // Investigate,
        // Explore,
        Search,

        Retrieve,
        // Gather,
        // Pickup,
        // Scavenge,

        Deliver,
        // Give,

        Bribe,
        // Pay,

        Hack,

        Steal,
        // Appropriate,
        // Recover,

        // Attack,
        // Murder,
        Kill,
    }

    [System.Serializable]
    public struct SentienceQuestParser
    {
        public string name;
        public string description;
        public SentienceQuestStageParser[] stages;
    }

    [System.Serializable]
    public struct SentienceQuestStageParser
    {
        public string objective;
        public string target;
        public string action;
    }

    [System.Serializable]
    public class SentienceQuestStage
    {
        public string Objective;
        public string targetString;
        public string actionString;
        public InteractionData InteractionData;
    }

    [System.Serializable]
    public class SentienceQuest
    {
        public string Name;
        public string Description;
        public string Location;
        [SerializeReference] public IdentityData Source;
        public List<SentienceQuestStage> Stages;

        public static async Awaitable<SentienceQuest> Generate(SentienceQuestParser parser, string location, IdentityData source, List<IdentityData> data)
        {
            List<string> ids = new();
            List<Item> spawnedItems = new();
            List<string> items = new();
            foreach (var id in data)
            {
                ids.Add($"{id.Name}|{id.Description}");
                if (id.Inventory != null)
                {
                    foreach (var i in id.Inventory.Items)
                    {
                        spawnedItems.Add(i);
                        items.Add($"{i.Name}|{i.Description}");
                    }
                }
            }

            SentienceQuest quest = new SentienceQuest();
            quest.Name = parser.name.Trim();
            quest.Source = source;
            quest.Description = parser.description.Trim();
            quest.Location = location.Trim();
            quest.Stages = new();
            foreach (var parserStage in parser.stages)
            {
                SentienceQuestStage stage = new SentienceQuestStage
                {
                    Objective = parserStage.objective.Trim(),
                    InteractionData = null,
                };

                // string itm = await SentienceManager.Instance.RagManager.GetMostSimilar(items, parserStage.target);
                // itm = itm.Split('|')[0];
                stage.targetString = parserStage.target.Trim();
                stage.actionString = parserStage.action.Trim();
                Item itemTarget = spawnedItems.FirstOrDefault(x => x.Name.ToLower() == parserStage.target.Trim().ToLower());
                if (itemTarget != null)
                {
                    bool foundItemTarget = false;
                    foreach (var identityTarget in data)
                    {
                        if (identityTarget.Inventory != null)
                        {
                            foreach (var item in identityTarget.Inventory.Items)
                            {
                                if (itemTarget == item)
                                {
                                    List<string> itemInteractions = new();
                                    foreach (var interaction in itemTarget.Type.Interactions) itemInteractions.Add($"{interaction.Name}|{interaction.Description}");
                                    string itemInteractionName = parserStage.action.Trim();
                                    itemInteractionName = await SentienceManager.Instance.RagManager.GetMostSimilar(itemInteractions, itemInteractionName);
                                    itemInteractionName = itemInteractionName.Split('|')[0];
                                    ItemInteraction itemInteraction = itemTarget.Type.Interactions.FirstOrDefault(x => x.Name == itemInteractionName);
                                    stage.InteractionData = new(itemTarget, itemInteraction, identityTarget, null);
                                    foundItemTarget = true;
                                    break;
                                }
                            }
                            if (foundItemTarget) break;
                        }
                    }
                }
                else
                {
                    string target = await SentienceManager.Instance.RagManager.GetMostSimilar(ids, parserStage.target.Trim());
                    target = target.Split('|')[0];
                    IdentityData identityTarget = data.FirstOrDefault(x => x.Name == target);

                    List<string> identityActions = new();
                    foreach (var interaction in identityTarget.Type.Interactions) identityActions.Add($"{interaction.Name}|{interaction.Description}");
                    string identityIntaractionName = parserStage.action.Trim();
                    identityIntaractionName = await SentienceManager.Instance.RagManager.GetMostSimilar(identityActions, identityIntaractionName);
                    identityIntaractionName = identityIntaractionName.Split('|')[0];
                    IdentityInteraction identityInteraction = identityTarget.Type.Interactions.FirstOrDefault(x => x.Name == identityIntaractionName);
                    stage.InteractionData = new(null, null, identityTarget, identityInteraction);
                }

                quest.Stages.Add(stage);
            }
            return quest;
        }
    }
}