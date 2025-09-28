using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    [System.Serializable]
    public struct SentienceCharacterParser
    {
        public string name;
        public string species;
        public string description;
        public string[] inventory;
    }

    [System.Serializable]
    public class SentienceCharacter
    {
        public string Name;
        public string Species;
        public string Description;
        public string Location;
        public List<string> Inventory = new();

        public SentienceCharacter(SentienceCharacterParser parser, string location)
        {
            Name = parser.name.Trim();
            Species = parser.species.Trim();
            Description = parser.description.Trim();
            Location = location.Trim();
            Inventory = new();
            if (parser.inventory != null)
            {
                foreach (var item in parser.inventory) Inventory.Add(item.Trim());
            }
        }

        public static async Awaitable<SentienceCharacter> GenerateSentienceCharacter(string description, string location)
        {
            string answer;
            string rules = "I will tell you the location and the description of a character and you must respond with a character that fits these parameters.\n" +
                           "You must only answer in the following JSON format:\n" +
                           "{\n" +
                           "\"name\": \"<the name of the character>\",\n" +
                           "\"species\": \"<the species of the character>\",\n" +
                           "\"description\": \"<a description of the character>\",\n" +
                           "\"inventory\": [\"<a JSON list of strings with the name of each individual item this character has.>\"]\n" +
                           "}";
            string msg = "";
            if (!string.IsNullOrWhiteSpace(description)) msg += $"Location: {location}";
            if (!string.IsNullOrWhiteSpace(description)) msg += $"Description: {description}";
            if (DungeonMaster.Instance.Cohere != null) answer = await CohereApi.Instance.AskQuestion(rules, msg, new List<CohereMessage>(), true);
            else answer = await DungeonMaster.Instance.AskQuestionToGenerator(rules, msg, null);
            Debug.Log($"Generated SentienceData!\n{answer}");
            try
            {
                SentienceCharacterParser parser = JsonConvert.DeserializeObject<SentienceCharacterParser>(answer);
                return new(parser, location);
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return await GenerateSentienceCharacter(description, location);
            }
        }
    }
}