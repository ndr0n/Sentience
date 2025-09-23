using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using LLMUnity;
using Newtonsoft.Json;
using MindTheatre;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.Serialization;

namespace Sentience
{
    public class DungeonMaster : MonoBehaviour
    {
        [Multiline] public string World = "Dungeons and dragons in a dystopian sci-fi setting.";
        [Multiline] public string Personality = "You are a Dungeon Master that creates dark stories filled with mind-bending intrigues.";
        public Lorebook Lorebook;

        [Header("Generator")]
        public LLMCharacter Generator;
        public CohereApi Cohere;

        bool awaitingResponse = false;
        readonly string jsonGrammarString = "root   ::= object\nvalue  ::= object | array | string | number | (\"true\" | \"false\" | \"null\") ws\n\nobject ::=\n  \"{\" ws (\n            string \":\" ws value\n    (\",\" ws string \":\" ws value)*\n  )? \"}\" ws\n\narray  ::=\n  \"[\" ws (\n            value\n    (\",\" ws value)*\n  )? \"]\" ws\n\nstring ::=\n  \"\\\"\" (\n    [^\"\\\\\\x7F\\x00-\\x1F] |\n    \"\\\\\" ([\"\\\\bfnrt] | \"u\" [0-9a-fA-F]{4}) # escapes\n  )* \"\\\"\" ws\n\nnumber ::= (\"-\"? ([0-9] | [1-9] [0-9]{0,15})) (\".\" [0-9]+)? ([eE] [-+]? [0-9] [1-9]{0,15})? ws\n\n# Optional space: by convention, applied in this grammar after literal chars when allowed\nws ::= | \" \" | \"\\n\" [ \\t]{0,20}";
        string systemPrompt = null;

        static DungeonMaster instance;
        public static DungeonMaster Instance
        {
            get
            {
                if (instance != null) return instance;
                return FindFirstObjectByType<DungeonMaster>();
            }
        }

        string lore = null;
        public string Lore
        {
            get
            {
                if (string.IsNullOrWhiteSpace(lore)) lore = Lorebook.GetLore();
                return lore;
            }
            set => lore = value;
        }

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                Lore = Lorebook.GetLore();
            }
            else Destroy(gameObject);
        }

        #region Generator

        void InitPrompt()
        {
            Generator.cachePrompt = true;
            Generator.prompt = "";
            Generator.grammarString = jsonGrammarString;
            Generator.seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
            systemPrompt = $"You are in {World} \n The main lore about the world is: {Lore} \n {Personality}";
            Generator.Warmup(systemPrompt);
        }

        public async Awaitable<string> AskQuestionToGenerator(string rules, string message, Callback<string> onReply)
        {
            while (awaitingResponse) await Awaitable.WaitForSecondsAsync(1f);
            awaitingResponse = true;
            if (string.IsNullOrWhiteSpace(systemPrompt)) InitPrompt();
            Generator.seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
            Generator.SetPrompt(rules, false);
            string response = await Generator.Chat(message, onReply, null, false);
            awaitingResponse = false;
            return response;
        }
        
        public async Awaitable<SentienceQuestParser> GenerateSentienceQuest(string details)
        {
            string rules = "I will tell you the characters that we have in scene and you will generate a quest that is engaging between those characters for the player to do.\n" +
                           "You must only answer in the following JSON format:\n" +
                           "{\n" +
                           "\"name\": \"<the name of the quest>\",\n" +
                           "\"description\": \"<a short description of the quest>\",\n" +
                           "\"stages\": [\"<\n" +
                           "this field must contain a JSON list containing each quest stage.\n" +
                           "Each individual quest stage on this list must have the following json format:" +
                           "{\n" +
                           "\"description\": \"<a short description of this quest stage.>\",\n" +
                           "\"location\": \"<the name of the location where the player must go during this quest stage.>\",\n" +
                           "\"target\": \"<the name of the target (item or character) that the player must perform the action on to complete this quest stage.>\",\n" +
                           "\"action\": \"<the name (one word) of the action that the player must perform on the target to complete this quest stage.>\",\n" +
                           "}\n" +
                           ">\"]\n" +
                           "}";
            string msg = "";
            msg += details;
            var answer = await AskQuestionToGenerator(rules, msg, null);
            Debug.Log($"Try Generate Quest!\n{answer}");
            SentienceQuestParser parser;
            try
            {
                parser = JsonConvert.DeserializeObject<SentienceQuestParser>(answer);
                return parser;
            }
            catch (Exception e)
            {
                Debug.Log(e);
                parser = await GenerateSentienceQuest(details);
            }
            return parser;
        }

        public async Awaitable<Sprite> GenerateSprite(Sprite example, string description)
        {
            try
            {
                string rules = "I will give you a description and you must generate a 32x32 texture, encoded in base64, that fits the given description.\n" +
                               "You must only answer in the following JSON format:\n" +
                               "{\n" +
                               "\"texture\": \"<a base64 string with a 32x32 texture that matches the description>\" \n" +
                               "}";
                // example = Instantiate(example);
                // byte[] exampleBytes = example.texture.GetRawTextureData();
                // Debug.Log($"EXAMPLE BYTES: {exampleBytes.Length}");
                // string exampleEncoded = Convert.ToBase64String(exampleBytes);
                // Debug.Log($"EXAMPLE ENCODED: {exampleEncoded}");
                string msg = "{\n" +
                             $"description: {description}\n" +
                             // $"example: {exampleEncoded}\n" +
                             "}";
                Debug.Log($"Try Generate Sprite Questioned!\n {msg}");
                var answer = await AskQuestionToGenerator(rules, msg, null);
                Debug.Log($"Try Generate Sprite Answered!\n {answer}");
                SentienceTextureParser parser = JsonConvert.DeserializeObject<SentienceTextureParser>(answer);
                string response = parser.texture;
                Sprite generated = Instantiate(example);
                byte[] responseBytes = Convert.FromBase64String(response);
                Debug.Log($"RESPONSE BYTES: {responseBytes.Length}");
                generated.texture.LoadRawTextureData(responseBytes);
                return generated;
            }
            catch (Exception e)
            {
                Debug.Log(e);
                throw;
            }
            return null;
        }

        #endregion
    }
}