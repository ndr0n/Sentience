using System.Collections.Generic;
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
    public struct SentienceQuestStage
    {
        public string Objective;
        public string Target;
        public SentienceQuestAction Action;
    }

    [System.Serializable]
    public class SentienceQuest
    {
        public string Name;
        public string Source;
        public string Description;
        public string Location;
        public List<SentienceQuestStage> Stages;

        public static async Awaitable<SentienceQuest> Generate(SentienceQuestParser parser, string location, string source)
        {
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
                    Target = parserStage.target,
                    Action = await SentienceManager.Instance.RagManager.GetMostSimilarSentienceQuestAction(parserStage.action)
                };
                quest.Stages.Add(stage);
            }
            return quest;
        }
    }
}