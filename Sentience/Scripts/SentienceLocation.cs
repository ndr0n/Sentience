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
        public List<IdentityData> LocationObjects;

        public SentienceLocationDetails(Vector3Int size, Vector3Int position, string description, List<IdentityData> locationObjects)
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
    public class SentienceLocation
    {
        public string Name = "";
        public string Description = "";
        public Faction Faction;
        // public List<string> Items = new();
        // public List<SentienceCharacter> Characters = new();
        // public List<IdentityData> Characters = new();
        public List<IdentityData> Objects = new();
        public List<IdentityData> Characters = new();
        public Vector3 Size = Vector3.one;
        public Vector3 Position = Vector3.zero;

        public static async Awaitable<SentienceLocation> Generate(SentienceLocationParser parser, List<IdentityData> objects)
        {
            System.Random random = new(Random.Range(int.MinValue, int.MaxValue));
            SentienceLocation location = new()
            {
                Name = parser.name,
                Description = parser.description,
            };
            location.Faction = await SentienceManager.Instance.RagManager.GetMostSimilarFaction(SentienceManager.Instance.FactionData, parser.faction);

            List<string> identityOptions = new();
            foreach (var type in location.Faction.FactionIdentity) identityOptions.Add($"{type.name}|{type.Description}");

            location.Characters = new();
            foreach (var character in parser.characters)
            {
                string similar = await SentienceManager.Instance.RagManager.GetMostSimilar(identityOptions, $"{character.species} - {character.name} - {character.description}");
                IdentityType it = location.Faction.FactionIdentity.FirstOrDefault(x => x.name == similar.Split('|')[0]);
                IdentityData id = IdentityData.Create(it, random, Vector3.zero, location.Name);
                PersonaData pd = await PersonaData.Generate(id, new(character, location.Name, location.Faction));
                location.Characters.Add(id);
            }

            List<string> itemOptions = new();
            foreach (var type in SentienceManager.Instance.ItemData.Items) itemOptions.Add($"{type.name}|{type.Description}");

            location.Objects = new();
            if (objects != null)
            {
                foreach (var obj in objects)
                {
                    location.Objects.Add(obj);
                }

                foreach (var item in parser.items)
                {
                    foreach (var obj in objects.OrderBy(x => random.Next()))
                    {
                        if (obj.Inventory != null)
                        {
                            obj.Inventory.Add(new(item, $"Found in {location.Name}.", 1, await ItemType.GetType(SentienceManager.Instance.ItemData, item)));
                            break;
                        }
                    }
                }
            }

            return location;
        }

        public static async Awaitable<SentienceLocation> GenerateLocationFromArea(string area, List<IdentityData> objects)
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
                SentienceLocation loc = await Generate(parser, objects);
                return loc;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return await GenerateLocationFromArea(area, objects);
            }
        }
    }
}