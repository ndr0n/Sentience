using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    public class QuestData
    {
        public string Name;
        public List<QuestStage> Stages = new();
        [HideInInspector] public string Location;

        public async Task InitFromSentienceQuest(SentienceQuest sentienceQuest, EntityData player, List<EntityData> entities)
        {
            Name = sentienceQuest.Name;
            // Source = sentienceQuest.Source;
            Location = sentienceQuest.Location;
            Stages = new List<QuestStage>();
            foreach (var stage in sentienceQuest.Stages)
            {
                QuestStage questStage = new();
                await questStage.InitFromSentienceQuestStage(stage, player, entities);
                Stages.Add(questStage);
            }
        }
    }
}