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

        public static async Awaitable<Location> GenerateAreaLocation(Area area, Vector3 size, Vector3 position, string description, List<EntityData> locationObjects)
        {
            Location location = await GenerateLocationData(area, size, position, description, locationObjects);
            if (location == null) location = await GenerateAreaLocation(area, size, position, description, locationObjects);
            location.Size = size;
            location.Position = position;
            return location;
        }

        public static async Awaitable<Location> GenerateLocationData(Area area, Vector3 size, Vector3 position, string details, List<EntityData> locationObjects)
        {
            string exeption = "";
            if (area.Location.Count > 0)
            {
                exeption += "We already have some locations. You must generate different locations.\n" +
                            "The locations we already have are:\n";
                foreach (var loc in area.Location)
                {
                    exeption += $"Location: {loc.Name} | Characters: ";
                    foreach (var character in loc.Characters)
                    {
                        Info info = character.Get<Info>();
                        exeption += $"{info.Name}, ";
                    }
                    exeption += $"\n";
                }
                exeption += $"Each location you generate must be unique and different from the ones we already have.\n";
            }
            string msg = $"{exeption}\n";
            if (!string.IsNullOrWhiteSpace(details)) msg += $"{details}\n";
            msg += $"The size of the generated location is {size.x} meters by {size.z} meters.";
            msg += $"Area: {area.Name}";

            // int characterAmount = Mathf.CeilToInt(((size.x / 4f) * (size.y / 4f)) / 4f);

            Debug.Log($"Generating area data for: {area.Name}");
            Location location = await SentienceLocation.GenerateLocationFromArea(size, position, msg, locationObjects);
            area.Location.Add(location);
            return location;
        }

        public static async Awaitable<SentienceQuest> GenerateAreaQuest(Area area, string details)
        {
            try
            {
                string msg = $"The Area of the quest is: {area}\n";
                msg += "The existing locations in this area are:\n";
                foreach (var location in area.Location)
                {
                    msg += $"Location: {location.Name} - {location.Description}\n";
                    msg += $"Location Faction: {location.Faction.Name} - {location.Faction.Description}\n";

                    if (location.Objects.Count > 0)
                    {
                        msg += $"Location Objects:\n";
                        foreach (var obj in location.Objects)
                        {
                            Info info = obj.Get<Info>();
                            msg += $"{info.Name}";
                            if (obj.Has<Inventory>())
                            {
                                Inventory inv = obj.Get<Inventory>();
                                if (inv.Items.Count > 0)
                                {
                                    msg += $" (Items: ";
                                    foreach (var item in inv.Items)
                                    {
                                        Info itemInfo = item.Item.Data.Get<Info>();
                                        msg += $"{itemInfo.Name}, ";
                                    }
                                    msg += $")";
                                }
                            }
                            msg += "\n";
                        }
                    }

                    if (location.Characters.Count > 0)
                    {
                        msg += $"Location Characters:\n";
                        foreach (var chr in location.Characters.OrderBy(x => Random.Range(int.MinValue, int.MaxValue)))
                        {
                            Info info = chr.Get<Info>();
                            msg += $"{info.Name}";
                            if (chr.Has<Inventory>())
                            {
                                Inventory inv = chr.Get<Inventory>();
                                if (inv.Items.Count > 0)
                                {
                                    msg += $" (Items: ";
                                    foreach (var item in inv.Items)
                                    {
                                        Info itemInfo = item.Item.Data.Get<Info>();
                                        msg += $"{itemInfo.Name}, ";
                                    }
                                    msg += $")";
                                }
                            }
                            msg += "\n";
                        }
                    }
                }
                msg += details;

                Debug.Log($"Generating quest data for: {area}");
                SentienceQuestParser parser = await DungeonMaster.Instance.GenerateSentienceQuest(msg);
                SentienceQuest quest = SentienceQuest.Parse(parser, area.Name);
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
                    string msg = $"Location: {location.Name} - {location.Description}\n";
                    msg += $"Location Faction: {location.Faction.Name} - {location.Faction.Description}\n";

                    if (location.Objects.Count > 0)
                    {
                        msg += $"Location Objects:\n";
                        foreach (var obj in location.Objects)
                        {
                            Info info = obj.Get<Info>();
                            msg += $"{info.Name}";
                            if (obj.Has<Inventory>())
                            {
                                Inventory inv = obj.Get<Inventory>();
                                if (inv.Items.Count > 0)
                                {
                                    msg += $" (Items: ";
                                    foreach (var item in inv.Items)
                                    {
                                        Info itemInfo = item.Item.Data.Get<Info>();
                                        msg += $"{itemInfo.Name}, ";
                                    }
                                    msg += $")";
                                }
                            }
                        }
                    }

                    if (location.Characters.Count > 0)
                    {
                        msg += $"Location Characters:\n";
                        foreach (var chr in location.Characters.OrderBy(x => Random.Range(int.MinValue, int.MaxValue)))
                        {
                            Info info = chr.Get<Info>();
                            msg += $"{info.Name}";
                            if (chr.Has<Inventory>())
                            {
                                Inventory inv = chr.Get<Inventory>();
                                if (inv.Items.Count > 0)
                                {
                                    msg += $" (Items: ";
                                    foreach (var item in inv.Items)
                                    {
                                        Info itemInfo = item.Item.Data.Get<Info>();
                                        msg += $"{itemInfo.Name}, ";
                                    }
                                    msg += $")";
                                }
                            }
                            msg += "\n";
                        }
                    }
                    msg += details;

                    msg += details;
                    Debug.Log($"Generating quest data for: {location.Name}");
                    SentienceQuestParser parser = await DungeonMaster.Instance.GenerateSentienceQuest(msg);
                    SentienceQuest quest = SentienceQuest.Parse(parser, location.Name);
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