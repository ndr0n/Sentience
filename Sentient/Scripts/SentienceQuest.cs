using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Sentience
{
    [System.Serializable]
    public struct SentienceQuestParser
    {
        public string name;
        public SentienceQuestStageParser[] stages;
    }

    [System.Serializable]
    public struct SentienceQuestStageParser
    {
        public string description;
        public string objective;
        public string target;
        public string action;
    }

    [System.Serializable]
    public class SentienceQuestStage
    {
        public string Description;
        public string Objective;
        public string Target;
        public string Action;
    }

    [System.Serializable]
    public class SentienceQuest
    {
        public string Name;
        public string Location;
        public List<SentienceQuestStage> Stages;

        public static SentienceQuest Parse(SentienceQuestParser parser, string location)
        {
            SentienceQuest quest = new SentienceQuest();
            quest.Name = parser.name?.Trim();
            quest.Location = location?.Trim();
            quest.Stages = new();
            foreach (var parserStage in parser.stages)
            {
                SentienceQuestStage stage = new()
                {
                    Description = parserStage.description?.Trim(),
                    Objective = parserStage.objective?.Trim(),
                    Target = parserStage.target?.Trim(),
                    Action = parserStage.action?.Trim()
                };
                quest.Stages.Add(stage);
            }

            return quest;
        }

        public static async Task<SentienceQuest> GenerateQuest(string areaName, string areaDescription, List<(string name, string description, List<EntityData> objects, List<EntityData> characters)> locations, string details)
        {
            try
            {
                string msg = $"The area of the quest is: {areaName}\n";
                msg += $"The description of this area is: {areaDescription}\n";
                msg += "The existing locations in this area are:\n";
                foreach (var location in locations)
                {
                    msg += $"Location: {location.name} - {location.description}\n";

                    if (location.objects.Count > 0)
                    {
                        msg += $"Location Objects:\n";
                        foreach (var obj in location.objects)
                        {
                            msg += $"{obj.Name}";
                            if (obj.Has<Inventory>())
                            {
                                Inventory inv = obj.Get<Inventory>();
                                if (inv.Items.Count > 0)
                                {
                                    msg += $" (Items: ";
                                    foreach (var item in inv.Items)
                                    {
                                        msg += $"{item.Item.Name}, ";
                                    }

                                    msg += $")";
                                }
                            }

                            msg += "\n";
                        }
                    }

                    if (location.characters.Count > 0)
                    {
                        msg += $"Location Characters:\n";
                        foreach (var chr in location.characters.OrderBy(x => Random.Range(int.MinValue, int.MaxValue)))
                        {
                            msg += $"{chr.Name}";
                            if (chr.Has<Inventory>())
                            {
                                Inventory inv = chr.Get<Inventory>();
                                if (inv.Items.Count > 0)
                                {
                                    msg += $" (Items: ";
                                    foreach (var slot in inv.Items)
                                    {
                                        msg += $"{slot.Item.Name}, ";
                                    }

                                    msg += $")";
                                }
                            }

                            msg += "\n";
                        }
                    }
                }

                msg += details;

                Debug.Log($"Generating quest data for: {areaName}");
                SentienceQuestParser parser = await DungeonMaster.Instance.GenerateSentienceQuest(msg);
                SentienceQuest quest = Parse(parser, areaName);
                return quest;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return await GenerateQuest(areaName, areaDescription, locations, details);
            }
        }
    }
}