using System;
using System.Collections.Generic;
using System.Linq;
using MindTheatre;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    [System.Serializable]
    public struct SentienceLocationParser
    {
        public string name;
        public string description;
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
        public Vector3 Size = Vector3.one;
        public Vector3 Position = Vector3.zero;
        public List<SentienceCharacter> Characters = new();

        public SentienceLocation(SentienceLocationParser parser)
        {
            Name = parser.name;
            Description = parser.description;
            Characters = new();
            foreach (var character in parser.characters) Characters.Add(new(character, Name));
        }

        public static async Awaitable<SentienceLocation> GenerateLocationFromArea(string area, int characterAmount)
        {
            string answer;
            string rules = "I will tell you the area and description of a location and you must respond with a location that exists within this area.\n" +
                           "You must only answer in the following JSON format:\n" +
                           "{\n" +
                           "\"name\": \"<the name of the location>\",\n" +
                           "\"description\": \"<the description of the location>\",\n" +
                           // "\"objects\": [\"<\n" +
                           // "this field must contain a JSON list containing each individual object that exists in this location.\n" +
                           // "Each individual object on this field (list) must have the following json format:" +
                           // "{\n" +
                           // "\"name\": \"<the name of the object>\",\n" +
                           // "\"type\": \"<the type of the object>\",\n" +
                           // "\"description\": \"<the description of the object>\",\n" +
                           // "}\n" +
                           // ">\"],\n" +
                           "\"characters\": [\"<\n" +
                           $"this field must contain a JSON list containing {characterAmount} characters that exist in this location.\n" +
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
                return new(parser);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return await GenerateLocationFromArea(area, characterAmount);
            }
        }
    }
}