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
        public SentienceQuestStageParser[] stages;
    }

    [System.Serializable]
    public struct SentienceQuestStageParser
    {
        public string description;
        public string objective;
        public string target;
        public string action;
    }

    [System.Serializable]
    public class SentienceQuestStage
    {
        public string Description;
        public string Objective;
        public string Target;
        public string Action;
    }

    [System.Serializable]
    public class SentienceQuest
    {
        public string Name;
        public string Location;
        public List<SentienceQuestStage> Stages;

        public static SentienceQuest Parse(SentienceQuestParser parser, string location)
        {
            SentienceQuest quest = new SentienceQuest();
            quest.Name = parser.name?.Trim();
            quest.Location = location?.Trim();
            quest.Stages = new();
            foreach (var parserStage in parser.stages)
            {
                SentienceQuestStage stage = new()
                {
                    Description = parserStage.description?.Trim(),
                    Objective = parserStage.objective?.Trim(),
                    Target = parserStage.target?.Trim(),
                    Action = parserStage.action?.Trim()
                };
                quest.Stages.Add(stage);
            }
            return quest;
        }
    }
}