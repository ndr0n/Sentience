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
        public Area Area;

        public static SentienceArea Create(string areaName, string areaDescription)
        {
            SentienceArea sentienceArea = CreateInstance<SentienceArea>();
            sentienceArea.Area = new(areaName, areaDescription);
            return sentienceArea;
        }

        public static async Awaitable GenerateSentienceAreaLocations(Area area, List<SentienceLocationDetails> locationDetails)
        {
            foreach (var detail in locationDetails)
            {
                if (area.Location.FirstOrDefault(x => (x.Size == detail.Size) && (x.Position == detail.Position)) == null)
                {
                    Location location = await GenerateAreaLocation(area, detail.Size, detail.Position, detail.Description, detail.LocationObjects);
                }
            }
        }

        public static async Awaitable<Location> GenerateAreaLocation(Area area, Vector3 size, Vector3 position, string description, List<IdentityData> objects)
        {
            Location location = await GenerateLocationData(area, size, position, description, objects);
            if (location == null) location = await GenerateAreaLocation(area, size, position, description, objects);
            location.Size = size;
            location.Position = position;
            return location;
        }

        public static async Awaitable<Location> GenerateLocationData(Area area, Vector3 size, Vector3 position, string details, List<IdentityData> objects)
        {
            string exeption = "";
            if (area.Location.Count > 0)
            {
                exeption += "We already have some locations. You must generate different locations.\n" +
                            "The locations we already have are:\n";
                foreach (var loc in area.Location)
                {
                    exeption += $"Location: {loc.Name} | Characters: ";
                    foreach (var sentient in loc.Characters) exeption += $"{sentient.Name}, ";
                    exeption += $"\n";
                }
                exeption += $"Each location you generate must be unique and different from the ones we already have.\n";
            }
            string msg = $"{exeption}\n";
            if (!string.IsNullOrWhiteSpace(details)) msg += $"{details}\n";
            msg += $"The size of the generated location is {size.x} meters by {size.z} meters.";
            msg += $"Area: {area}";

            // int characterAmount = Mathf.CeilToInt(((size.x / 4f) * (size.y / 4f)) / 4f);

            Debug.Log($"Generating area data for: {area}");
            Location location = await SentienceLocation.GenerateLocationFromArea(size, position, msg, objects);
            area.Location.Add(location);
            return location;
        }

        public static async Awaitable<SentienceQuest> GenerateAreaQuest(Area area, string details)
        {
            try
            {
                List<IdentityData> data = new();
                string msg = $"The Area of the quest is: {area}\n";
                msg += "The existing locations in this area are:\n";
                IdentityData source = null;
                foreach (var location in area.Location)
                {
                    msg += $"Location: {location.Name} - {location.Description}\n";

                    msg += $"Location Faction: {location.Faction.Name} - {location.Faction.Description}\n";

                    if (location.Objects.Count > 0)
                    {
                        msg += $"Location Objects:\n";
                        foreach (var obj in location.Objects)
                        {
                            msg += $"{obj.Name}";
                            data.Add(obj);
                        }

                        msg += $"Location Items:\n";
                        foreach (var obj in data)
                        {
                            if (obj.Inventory != null)
                            {
                                foreach (var item in obj.Inventory.Items)
                                {
                                    msg += $"{item.Name}\n";
                                }
                            }
                        }
                    }

                    if (location.Characters.Count > 0)
                    {
                        msg += $"Location Characters:\n";
                        foreach (var character in location.Characters.OrderBy(x => Random.Range(int.MinValue, int.MaxValue)))
                        {
                            msg += $"{character.Name}\n";
                            data.Add(character);
                            source = character;
                        }
                        msg += $"the character that will give the quest to the player is: {source.Name}.\n";
                    }
                }
                msg += details;

                Debug.Log($"Generating quest data for: {area}");
                SentienceQuestParser parser = await DungeonMaster.Instance.GenerateSentienceQuest(msg);
                SentienceQuest quest = await SentienceQuest.Generate(parser, area.Name, source, data);
                area.Quests.Add(quest);
                return quest;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            return new();
        }

        public static async Awaitable<SentienceQuest> GenerateRandomLocationQuest(Area area, string details)
        {
            try
            {
                foreach (var location in area.Location.OrderBy(x => Random.Range(int.MinValue, int.MaxValue)))
                {
                    string msg = $"The location of the quest is: {location.Name} - {location.Description}\n";
                    msg += $"Location Faction: {location.Faction.Name} - {location.Faction.Description}\n";

                    List<IdentityData> data = new();
                    if (location.Objects.Count > 0)
                    {
                        msg += $"Location Objects:\n";
                        foreach (var obj in location.Objects)
                        {
                            msg += $"{obj}";
                            data.Add(obj);
                        }

                        msg += $"Location Items:\n";
                        foreach (var obj in data)
                        {
                            if (obj.Inventory != null)
                            {
                                foreach (var item in obj.Inventory.Items)
                                {
                                    msg += $"{item.Name}\n";
                                }
                            }
                        }
                    }
                    IdentityData source = null;
                    if (location.Characters.Count > 0)
                    {
                        msg += $"Location Characters:\n";
                        foreach (var character in location.Characters.OrderBy(x => Random.Range(int.MinValue, int.MaxValue)))
                        {
                            msg += $"{character.Name}\n";
                            data.Add(character);
                            source = character;
                        }
                        msg += $"the character that will give the quest to the player is: {source.Name}.\n";
                    }
                    msg += details;
                    Debug.Log($"Generating quest data for: {location.Name}");
                    SentienceQuestParser parser = await DungeonMaster.Instance.GenerateSentienceQuest(msg);
                    SentienceQuest quest = await SentienceQuest.Generate(parser, location.Name, source, data);
                    area.Quests.Add(quest);
                    return quest;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            return null;
        }

        SentienceArea CreateNewSentienceArea(string worldName, string areaDescription, Vector2Int worldPosition)
        {
            SentienceArea sentienceArea = Create(worldName, areaDescription);
            sentienceArea.name = worldName;
            SerializeSentienceArea(sentienceArea, worldName, worldPosition);
            return sentienceArea;
        }

        SentienceArea CreateSentienceAreaFromTemplate(SentienceArea template, string worldName, Vector2Int worldPosition)
        {
            SentienceArea sentienceArea = Instantiate(template);
            sentienceArea.name = worldName;
            SerializeSentienceArea(sentienceArea, worldName, worldPosition);
            return sentienceArea;
        }

        public void SerializeSentienceArea(SentienceArea sentienceArea, string worldName, Vector2Int worldPosition)
        {
#if UNITY_EDITOR
            string path = AssetDatabase.GenerateUniqueAssetPath($"Assets/Modules/World/Data/Worlds/{worldName}/{worldName}_SA_x{worldPosition.x}y{worldPosition.y}.asset");
            AssetDatabase.CreateAsset(sentienceArea, path);
            AssetDatabase.SaveAssets();
#endif
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(SentienceArea))]
    public class SentienceArea_Editor : Editor
    {
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
                SentienceArea.GenerateLocationData(SentienceArea.Area, new Vector3(5, 5, 0), Vector3.zero, "", null);
            }
            if (GUILayout.Button("Generate Area Quest"))
            {
                SentienceArea.GenerateAreaQuest(SentienceArea.Area, "");
            }
            if (GUILayout.Button("Generate Random Location Quest"))
            {
                SentienceArea.GenerateRandomLocationQuest(SentienceArea.Area, "");
            }
        }
    }
#endif
}