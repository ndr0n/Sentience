using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    [System.Serializable]
    public class Quest
    {
        public int Stage;
        public QuestData Data;
        public Action<Quest> OnAdvanceStage;
        public Color Color;

        public Quest(QuestData data, int stage, Action<Quest> onAdvanceStage, Color color)
        {
            Data = data;
            Stage = stage;
            OnAdvanceStage = onAdvanceStage;
            Color = color;
        }
    }
}