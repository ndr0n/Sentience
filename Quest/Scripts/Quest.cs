using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    public class Quest
    {
        [SerializeReference] public SentienceQuest QuestData;
        public int Stage;

        public Quest(SentienceQuest questData, int stage)
        {
            QuestData = questData;
            Stage = stage;
        }
    }
}