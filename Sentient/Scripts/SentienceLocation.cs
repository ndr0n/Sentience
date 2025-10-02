using System;
using System.Collections.Generic;
using System.Linq;
using MindTheatre;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Sentience
{
    [System.Serializable]
    public struct SentienceLocationDetails
    {
        public Vector3Int Size;
        public Vector3Int Position;
        public string Description;
        public List<EntityData> LocationObjects;

        public SentienceLocationDetails(Vector3Int size, Vector3Int position, string description, List<EntityData> locationObjects)
        {
            Size = size;
            Position = position;
            Description = description;
            LocationObjects = locationObjects;
        }
    }

    [System.Serializable]
    public struct SentienceLocationParser
    {
        public string name;
        public string description;
        public string faction;
        public string[] items;
        public SentienceCharacterParser[] characters;
    }

    [System.Serializable]
    public struct SentienceLocationObject
    {
        public string name;
        public string type;
        public string description;
    }

    [System.Serializable]
    public static class SentienceLocation
    {
        public static async Awaitable<Location> Generate(SentienceLocationParser parser, Vector3 size, Vector3 position, List<EntityData> locationObjects)
        {
            try
            {
                System.Random random = new(Random.Range(int.MinValue, int.MaxValue));
                Location location = new()
                {
                    Name = parser.name,
                    Description = parser.description,
                    Faction = await SentienceManager.Instance.RagManager.GetMostSimilarFaction(SentienceManager.Instance.FactionDatabase, parser.faction),
                    Size = size,
                    Position = position,
                };

                List<string> identityOptions = new();
                foreach (var type in location.Faction.FactionEntity) identityOptions.Add($"{type.name}|{type.Description}");

                if (parser.characters != null)
                {
                    location.Characters = new();
                    foreach (var character in parser.characters)
                    {
                        string similar = await SentienceManager.Instance.RagManager.GetMostSimilar(identityOptions, $"{character.species} | {character.name} | {character.description}");
                        similar = similar.Split('|')[0];
                        EntityType spawnType = location.Faction.FactionEntity.FirstOrDefault(x => x.name == similar);

                        Vector3 spawnPosition = new Vector3(
                            random.Next(Mathf.FloorToInt(location.Position.x), Mathf.FloorToInt(location.Position.x + location.Size.x)),
                            random.Next(Mathf.FloorToInt(location.Position.y), Mathf.FloorToInt(location.Position.y + location.Size.y)),
                            random.Next(Mathf.FloorToInt(location.Position.z), Mathf.FloorToInt(location.Position.z + location.Size.z))
                        );
                        SentienceCharacter sc = new(character, location.Name);
                        EntityData characterData = new(sc.Name, sc.Description, spawnType, random);

                        ID id = characterData.GetData<ID>();
                        id.Position = spawnPosition;
                        characterData.SetData(id);

                        Identity identity = characterData.Get<Identity>();
                        await identity.LoadSentienceCharacter(sc, location.Faction, random);

                        location.Characters.Add(characterData);
                    }
                }

                location.Objects = new();
                if (locationObjects != null)
                {
                    foreach (var obj in locationObjects)
                    {
                        location.Objects.Add(obj);
                    }

                    foreach (var item in parser.items)
                    {
                        foreach (var obj in locationObjects.OrderBy(x => random.Next()))
                        {
                            if (obj.Has<Inventory>())
                            {
                                Inventory inv = obj.Get<Inventory>();
                                EntityData itemData = new(item, $"Found in {location.Name}.", await SentienceManager.Instance.RagManager.GetMostSimilarItem(SentienceManager.Instance.ItemDatabase, item), random);
                                Item itm = itemData.Get<Item>();
                                inv.Add(itm);
                                break;
                            }
                        }
                    }
                }

                Debug.Log($"Location Generated! {location.Name}");

                return location;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        public static async Awaitable<Location> GenerateLocationFromArea(Vector3 size, Vector3 position, string area, List<EntityData> locationObjects)
        {
            string answer;
            string rules = "I will tell you the area and description of a location and you must respond with a location that exists within this area.\n" +
                           "You must only answer in the following JSON format:\n" +
                           "{\n" +
                           "\"name\": \"<the name of the location>\",\n" +
                           "\"description\": \"<the description of the location>\",\n" +
                           "\"faction\": \"<the faction that currently controls the location>\",\n" +
                           "\"items\": [\"<a JSON list of strings with the name of each individual item that is present in this location.>\"],\n" +
                           "\"characters\": [\"<\n" +
                           $"a JSON list containing characters from the controlling faction that exist in this location.\n" +
                           "Each individual character on this field (list) must have the following JSON format:" +
                           "{\n" +
                           "\"name\": \"<the name of the character>\",\n" +
                           "\"species\": \"<the species of the character>\",\n" +
                           "\"description\": \"<the description of the character>\" \n" +
                           "\"inventory\": [\"<a JSON list of strings with the name of each individual item that this character is carrying.>\"]\n" +
                           "}";

            if (DungeonMaster.Instance.Cohere != null) answer = await CohereApi.Instance.AskQuestion(rules, area, new List<CohereMessage>(), true);
            else answer = await DungeonMaster.Instance.AskQuestionToGenerator(rules, area, null);
            Debug.Log($"Generated Location!\n{answer}");
            try
            {
                SentienceLocationParser parser = JsonConvert.DeserializeObject<SentienceLocationParser>(answer);
                Location loc = await Generate(parser, size, position, locationObjects);
                return loc;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return await GenerateLocationFromArea(size, position, area, locationObjects);
            }
        }
    }
}