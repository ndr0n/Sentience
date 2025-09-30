using System.Collections.Generic;
using Unity.Entities;
using Unity.VisualScripting;

namespace Sentience
{
    [System.Serializable]
    public class Journal : EntityComponent
    {
        public List<Quest> Quests;
    }

    [System.Serializable]
    public class JournalAuthoring : EntityAuthoring
    {
        public override IEntityComponent Spawn(System.Random random)
        {
            Journal journal = new();
            journal.Quests = new();
            return journal;
        }
    }
}