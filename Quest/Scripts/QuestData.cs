using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    public class QuestData
    {
        public string Name;
        public string Source;
        public string Location;
        public List<QuestStage> Stages = new();

        public async Awaitable InitFromSentienceQuest(SentienceQuest sentienceQuest, Entity player, List<Entity> entities)
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