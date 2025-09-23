using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

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

            int characterAmount = Mathf.CeilToInt(((size.x / 4f) * (size.y / 4f)) / 4f);
            Debug.Log($"Generating area data for: {Area}");
            SentienceLocation location = await SentienceLocation.GenerateLocationFromArea(msg, characterAmount);
            LocationData.Add(location);
            return location;
        }

        public async Awaitable<SentienceQuest> GenerateAreaQuest(string details)
        {
            try
            {
                string msg = "The existing locations are:\n";
                foreach (var location in LocationData)
                {
                    msg += $"Location: {location.Name} - {location.Description}\n";
                    // msg += $"Location Objects:\n";
                    // foreach (var objkt in location.Objects) msg += $"Name: {objkt.name} | Type: {objkt.type} | Description: {objkt.description}\n";
                    msg += $"Location Characters:\n";
                    foreach (var character in location.Characters) msg += $"Name: {character.Name} | Description: {character.Description}\n";
                }
                msg += details;
                Debug.Log($"Generating quest data for: {Area}");
                SentienceQuestParser parser = await DungeonMaster.Instance.GenerateSentienceQuest(msg);
                SentienceQuest quest = new SentienceQuest(parser);
                Quests.Add(quest);
                return quest;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            return new();
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
        }
    }
#endif
}