using System.Collections.Generic;
using Unity.VisualScripting;

namespace Sentience
{
    [System.Serializable]
    public class Journal : Component
    {
        public List<Quest> Quests = new();

        public Journal(EntityData data) : base(data)
        {
            _data = data;
        }
    }
}