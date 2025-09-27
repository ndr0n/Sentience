using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
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
        public string Source;
        public List<SentienceQuestStage> Stages;

        public static async Awaitable<SentienceQuest> Generate(SentienceQuestParser parser, string location, IdentityData source, List<EntityData> data)
        {
            if (data.Count == 0)
            {
                Debug.Log($"NO ENTITIES FOR QUEST");
                return null;
            }

            SentienceQuest quest = new SentienceQuest();
            quest.Name = parser.name.Trim();
            quest.Source = source.Name;
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

                stage.targetString = parserStage.target;
                stage.actionString = parserStage.action;

                EntityData eTarget;
                if (!string.IsNullOrEmpty(stage.targetString))
                {
                    List<string> entities = new();
                    foreach (var d in data) entities.Add($"{d.Name}|{d.Description}");
                    stage.targetString = stage.targetString.Trim();
                    string entityTarget = await SentienceManager.Instance.RagManager.GetMostSimilar(entities, stage.targetString);
                    entityTarget = entityTarget.Split('|')[0];
                    eTarget = data.FirstOrDefault(x => x.Name == entityTarget);
                }
                else
                {
                    eTarget = data[Random.Range(0, data.Count)];
                    stage.targetString = eTarget.Name;
                }

                Interaction interaction;
                if (!string.IsNullOrWhiteSpace(stage.actionString))
                {
                    stage.actionString = stage.actionString.Trim();
                    List<string> interactions = new();
                    foreach (var inter in eTarget.Type.Interactions) interactions.Add($"{inter.Name}|{inter.Description}");
                    string targetInteractionName = await SentienceManager.Instance.RagManager.GetMostSimilar(interactions, stage.actionString);
                    targetInteractionName = targetInteractionName.Split('|')[0];
                    interaction = eTarget.Type.Interactions.FirstOrDefault(x => x.Name == targetInteractionName);
                }
                else
                {
                    interaction = eTarget.Type.Interactions[Random.Range(0, eTarget.Type.Interactions.Count)];
                    stage.actionString = interaction.Name;
                }

                stage.InteractionData = new(eTarget.Name, interaction);
                quest.Stages.Add(stage);
            }
            return quest;
        }
    }
}