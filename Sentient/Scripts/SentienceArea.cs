using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Sentience
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "SentienceArea", menuName = "Sentience/SentienceArea")]
    public class SentienceArea : ScriptableObject
    {
        public string Area = "Sigil, the city of doors.";
        public List<SentienceLocation> LocationData = new();
        public List<SentienceQuest> Quests = new();

        public static SentienceArea Create(string area)
        {
            SentienceArea sentienceArea = CreateInstance<SentienceArea>();
            sentienceArea.Area = area;
            sentienceArea.name = area;
            sentienceArea.LocationData = new();
            return sentienceArea;
        }

        public async Awaitable<SentienceLocation> GenerateAreaLocation(Vector3 size, Vector3 position, string description)
        {
            SentienceLocation location = await GenerateLocationData(size, description);
            if (location == null) location = await GenerateAreaLocation(size, position, description);
            location.Size = size;
            location.Position = position;
            return location;
        }

        public async Awaitable<SentienceLocation> GenerateLocationData(Vector3 size, string details)
        {
            string exeption = "";
            if (LocationData.Count > 0)
            {
                exeption += "We already have some locations. You must generate different locations.\n" +
                            "The locations we already have are:\n";
                foreach (var loc in LocationData)
                {
                    exeption += $"Location: {loc.Name} | Characters: ";
                    foreach (var sentient in loc.Characters) exeption += $"{sentient.Name}, ";
                    exeption += $"\n";
                }
                exeption += $"Each location you generate must be unique and different from the ones we already have.\n";
            }
            string msg = $"{exeption}\n";
            if (!string.IsNullOrWhiteSpace(details)) msg += $"{details}\n";
            msg += $"The size of the generated location is {size.x} meters by {size.y} meters.";
            msg += $"Area: {Area}";

            // int characterAmount = Mathf.CeilToInt(((size.x / 4f) * (size.y / 4f)) / 4f);

            Debug.Log($"Generating area data for: {Area}");
            SentienceLocation location = await SentienceLocation.GenerateLocationFromArea(msg);
            LocationData.Add(location);
            return location;
        }

        public async Awaitable<SentienceQuest> GenerateAreaQuest(string details)
        {
            try
            {
                string msg = $"The Area of the quest is: {Area}\n";
                msg += "The existing locations in this area are:\n";
                SentienceCharacter source = null;
                foreach (var location in LocationData)
                {
                    msg += $"Location: {location.Name} - {location.Description}\n";
                    msg += $"Location Faction: {location.Faction.Name} - {location.Faction.Description}\n";
                    msg += $"Location Items:\n";
                    foreach (var item in location.Items)
                    {
                        msg += $"{item}\n";
                    }
                    msg += $"Location Characters:\n";
                    foreach (var character in location.Characters.OrderBy(x => Random.Range(int.MinValue, int.MaxValue)))
                    {
                        source = character;
                        msg += $"{character.Name}\n";
                    }
                }
                msg += $"the character that will give the quest to the player is: {source.Name}.\n";
                msg += details;

                Debug.Log($"Generating quest data for: {Area}");
                SentienceQuestParser parser = await DungeonMaster.Instance.GenerateSentienceQuest(msg);
                SentienceQuest quest = await SentienceQuest.Generate(parser, Area, source.Name);
                Quests.Add(quest);
                return quest;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            return new();
        }

        public async Awaitable<SentienceQuest> GenerateRandomLocationQuest(string details)
        {
            try
            {
                foreach (var location in LocationData.OrderBy(x => Random.Range(int.MinValue, int.MaxValue)))
                {
                    string msg = $"The location of the quest is: {location.Name} - {location.Description}\n";
                    msg += $"Location Faction: {location.Faction.Name} - {location.Faction.Description}\n";
                    msg += $"Location Items:\n";
                    foreach (var item in location.Items)
                    {
                        msg += $"{item}\n";
                    }
                    msg += $"Location Characters:\n";
                    SentienceCharacter source = null;
                    foreach (var character in location.Characters.OrderBy(x => Random.Range(int.MinValue, int.MaxValue)))
                    {
                        msg += $"{character.Name}\n";
                        source = character;
                    }
                    msg += $"the character that will give the quest to the player is: {source.Name}.\n";
                    msg += details;

                    Debug.Log($"Generating quest data for: {location.Name}");
                    SentienceQuestParser parser = await DungeonMaster.Instance.GenerateSentienceQuest(msg);
                    SentienceQuest quest = await SentienceQuest.Generate(parser, location.Name, source.Name);
                    Quests.Add(quest);
                    return quest;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            return null;
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(SentienceArea))]
    public class SentienceAreaData_Editor : Editor
    {
        [FormerlySerializedAs("SentienceAreaData")]
        public SentienceArea SentienceArea;

        void OnEnable()
        {
            if (target == null) return;
            SentienceArea = (SentienceArea) target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("Generate Area Location"))
            {
                SentienceArea.GenerateLocationData(new Vector3(5, 5, 0), "");
            }
            if (GUILayout.Button("Generate Area Quest"))
            {
                SentienceArea.GenerateAreaQuest("");
            }
            if (GUILayout.Button("Generate Random Location Quest"))
            {
                SentienceArea.GenerateRandomLocationQuest("");
            }
        }
    }
#endif
}