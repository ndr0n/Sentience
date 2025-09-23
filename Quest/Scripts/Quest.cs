using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Quest", menuName = "Sentience/Quest")]
    public class Quest : ScriptableObject
    {
        public string Name = "";
        public string Description = "";

        public static Quest Generate(SentienceQuest sentienceQuest)
        {
            Quest quest = CreateInstance<Quest>();
            quest.Name = sentienceQuest.Name;
            quest.Description = sentienceQuest.Description;
            return quest;
        }
    }
}