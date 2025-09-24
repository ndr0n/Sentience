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
        public string Item;
        public string targetString;
        public IdentityData Target;
        public string interactionString;
        public IdentityInteraction Interaction;
    }

    [System.Serializable]
    public class SentienceQuest
    {
        public string Name;
        public IdentityData Source;
        public string Description;
        public string Location;
        public List<SentienceQuestStage> Stages;

        public static async Awaitable<SentienceQuest> Generate(SentienceQuestParser parser, string location, IdentityData source, List<IdentityData> data)
        {
            List<string> ids = new();
            List<Item> identityItems = new();
            List<string> items = new();
            foreach (var id in data)
            {
                ids.Add($"{id.Name}|{id.Description}");
                foreach (var i in id.Inventory.Items)
                {
                    identityItems.Add(i);
                    items.Add($"{i.Name}|{i.Description}");
                }
            }

            SentienceQuest quest = new SentienceQuest();
            quest.Name = parser.name;
            quest.Source = source;
            quest.Description = parser.description;
            quest.Location = location;
            quest.Stages = new();
            foreach (var parserStage in parser.stages)
            {
                SentienceQuestStage stage = new SentienceQuestStage
                {
                    Objective = parserStage.objective,
                };

                // string itm = await SentienceManager.Instance.RagManager.GetMostSimilar(items, parserStage.target);
                // itm = itm.Split('|')[0];
                stage.targetString = parserStage.target;
                Item it = identityItems.FirstOrDefault(x => x.Name.ToLower() == parserStage.target.ToLower());
                if (it != null)
                {
                    stage.Item = it.Name;
                    foreach (var target in data)
                    {
                        foreach (var item in target.Inventory.Items)
                        {
                            if (it == item)
                            {
                                stage.Target = target;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    string target = await SentienceManager.Instance.RagManager.GetMostSimilar(ids, parserStage.target);
                    target = target.Split('|')[0];
                    stage.Target = data.FirstOrDefault(x => x.Name == target);
                }

                List<string> actions = new();
                foreach (var action in stage.Target.Type.Interactions) actions.Add($"{action.Name}|{action.Description}");

                stage.interactionString = parserStage.action;
                string interactionName = parserStage.action;
                if (!string.IsNullOrWhiteSpace(stage.Item)) interactionName = "Retrieve";

                interactionName = await SentienceManager.Instance.RagManager.GetMostSimilar(actions, interactionName);
                interactionName = interactionName.Split('|')[0];
                stage.Interaction = stage.Target.Type.Interactions.FirstOrDefault(x => x.Name == interactionName);

                quest.Stages.Add(stage);
            }
            return quest;
        }
    }
}