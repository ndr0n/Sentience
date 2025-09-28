using System.Collections.Generic;
using Unity.VisualScripting;

namespace Sentience
{
    [System.Serializable]
    public class Journal : IEntityComponent
    {
        EntityData _data;
        public EntityData Data => _data;

        public void Init(EntityData data, System.Random random)
        {
            _data = data;
            if (Quests == null) Quests = new();
        }

        public List<Quest> Quests;
    }

    [System.Serializable]
    public class JournalAuthoring : EntityComponentAuthoring
    {
        public override IEntityComponent Spawn(System.Random random)
        {
            Journal journal = new();
            return journal;
        }
    }
}