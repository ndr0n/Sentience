using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    [System.Serializable]
    public struct SentienceLocationDetails
    {
        public Vector3Int Size;
        public Vector3Int Position;
        public string Description;

        public SentienceLocationDetails(Vector3Int size, Vector3Int position, string description)
        {
            Size = size;
            Position = position;
            Description = description;
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
        public Vector3 Size = Vector3.one;
        public Vector3 Position = Vector3.zero;
        public List<string> Items = new();
        public List<SentienceCharacter> Characters = new();

        public static async Awaitable<SentienceLocation> Generate(SentienceLocationParser parser)
        {
            SentienceLocation location = new()
            {
                Name = parser.name,
                Description = parser.description,
            };
            location.Faction = await SentienceManager.Instance.RagManager.GetMostSimilarFaction(SentienceManager.Instance.FactionData, parser.faction);
            location.Items = new();
            foreach (var item in parser.items) location.Items.Add(item);
            location.Characters = new();
            foreach (var character in parser.characters) location.Characters.Add(new(character, location.Name, location.Faction));
            return location;
        }

        public static async Awaitable<SentienceLocation> GenerateLocationFromArea(string area)
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
                SentienceLocation loc = await Generate(parser);
                return loc;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return await GenerateLocationFromArea(area);
            }
        }
    }
}