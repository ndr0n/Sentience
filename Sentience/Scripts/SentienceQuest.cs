using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    public enum SentienceQuestAction
    {
        Speak,
        Chat,
        Talk,
        Approach,

        Investigate,
        Explore,
        Search,

        Retrieve,
        Gather,
        Pickup,
        Scavenge,

        Deliver,
        Give,

        Bribe,
        Pay,

        Hack,

        Steal,
        Appropriate,
        Recover,

        Attack,
        Murder,
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
        public string description;
        // public string location;
        public string target;
        public string action;
    }

    [System.Serializable]
    public struct SentienceQuestStage
    {
        public string Description;
        // public string Location;
        public string Target;
        public SentienceQuestAction action;
    }

    [System.Serializable]
    public struct SentienceQuest
    {
        public string Name;
        public string Description;
        public List<SentienceQuestStage> Stages;

        public static async Awaitable<SentienceQuest> Generate(SentienceQuestParser parser)
        {
            SentienceQuest quest = new SentienceQuest();
            quest.Name = parser.name;
            quest.Description = parser.description;
            quest.Stages = new();
            foreach (var parserStage in parser.stages)
            {
                SentienceQuestStage stage = new SentienceQuestStage
                {
                    Description = parserStage.description,
                    // Location = parserStage.location,
                    Target = parserStage.target,
                    action = await SentienceManager.Instance.RagManager.GetMostSimilarSentienceQuestAction(parserStage.action)
                };
                quest.Stages.Add(stage);
            }
            return quest;
        }
    }
}