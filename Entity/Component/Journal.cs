using System.Collections.Generic;
using Unity.VisualScripting;

namespace Sentience
{
    [System.Serializable]
    public class Journal : EntityComponent
    {
        public List<Quest> Quests = new();
        public List<Information> Information = new();

        public bool HasQuest(QuestData questData)
        {
            foreach (var quest in Quests)
            {
                if (quest.Data == questData) return true;
            }

            return false;
        }

        public Quest GetQuest(QuestData questData)
        {
            foreach (var quest in Quests)
            {
                if (quest.Data == questData) return quest;
            }

            return null;
        }
    }

    [System.Serializable]
    public class JournalAuthoring : EntityAuthoring
    {
        public override EntityComponent Spawn(System.Random random)
        {
            Journal journal = new();
            journal.Quests = new();
            return journal;
        }
    }
}