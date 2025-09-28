using System.Collections.Generic;
using Unity.VisualScripting;

namespace Sentience
{
    [System.Serializable]
    public class Journal : EntityComponent
    {
        public List<Quest> Quests = new();
        
        public override void OnInit(EntityData data, System.Random random)
        {
        }
    }
}