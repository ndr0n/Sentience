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
        public SentienceQuestStage[] stages;
    }

    [System.Serializable]
    public struct SentienceQuestStage
    {
        public string description;
        public string location;
        public string target;
        public string action;
    }

    [System.Serializable]
    public struct SentienceQuest
    {
        public string Name;
        public string Description;
        public SentienceQuestStage[] Stages;

        public SentienceQuest(SentienceQuestParser parser)
        {
            Name = parser.name;
            Description = parser.description;
            Stages = parser.stages;
        }
    }
}