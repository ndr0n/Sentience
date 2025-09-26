using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    [System.Serializable]
    public class QuestData
    {
        public SentienceQuest Data;

        public QuestData(SentienceQuest data)
        {
            Data = data;
        }
    }
}