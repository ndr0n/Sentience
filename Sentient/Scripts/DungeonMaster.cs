using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using LLMUnity;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.Serialization;

namespace Sentience
{
    public class DungeonMaster : MonoBehaviour
    {
        [Multiline] public string World = "Dungeons and dragons in a dystopian sci-fi setting.";

        [Multiline] public string Personality =
            "You are a Dungeon Master that creates dark stories filled with mind-bending intrigues.";

        public Lorebook Lorebook;

        [Header("Generator")]
        public LLMAgent Generator;
        public CohereApi Cohere;

        bool awaitingResponse = false;
        bool quit = false;

        readonly string jsonGrammarString =
            "root   ::= object\nvalue  ::= object | array | string | number | (\"true\" | \"false\" | \"null\") ws\n\nobject ::=\n  \"{\" ws (\n            string \":\" ws value\n    (\",\" ws string \":\" ws value)*\n  )? \"}\" ws\n\narray  ::=\n  \"[\" ws (\n            value\n    (\",\" ws value)*\n  )? \"]\" ws\n\nstring ::=\n  \"\\\"\" (\n    [^\"\\\\\\x7F\\x00-\\x1F] |\n    \"\\\\\" ([\"\\\\bfnrt] | \"u\" [0-9a-fA-F]{4}) # escapes\n  )* \"\\\"\" ws\n\nnumber ::= (\"-\"? ([0-9] | [1-9] [0-9]{0,15})) (\".\" [0-9]+)? ([eE] [-+]? [0-9] [1-9]{0,15})? ws\n\n# Optional space: by convention, applied in this grammar after literal chars when allowed\nws ::= | \" \" | \"\\n\" [ \\t]{0,20}";

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

        void OnDestroy()
        {
            quit = true;
            Generator.CancelRequests();
            awaitingResponse = false;
        }

        #region Generator

        async Task InitPrompt()
        {
            Generator.systemPrompt = "";
            Generator.SetGrammar(jsonGrammarString);
            systemPrompt = $"You are in {World} \n The main lore about the world is: {Lore} \n {Personality}";
            await Generator.Warmup(systemPrompt);
        }

        public async Task<string> AskQuestionToGenerator(string rules, string message, Action<string> onReply)
        {
            while (awaitingResponse) await Task.Delay(500);
            if (quit) return null;

            Debug.Log($"DM - Question: \n{rules}\n{message}");

            awaitingResponse = true;
            if (string.IsNullOrWhiteSpace(systemPrompt)) await InitPrompt();
            Generator.seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
            Generator.systemPrompt = $"{rules}";
            string response = await Generator.Chat(message, onReply, null, false);
            awaitingResponse = false;

            Debug.Log($"DM - Response: \n{response}");

            return response;
        }

        public async Task<SentienceQuestParser> GenerateSentienceQuest(string details)
        {
            SentienceQuestParser parser;
            try
            {
                string rules =
                    "I will tell you the entities that we have in an area and must respond with a generated engaging quest for the player to complete.\n" +
                    "You must only answer with a quest in the following JSON format:\n" +
                    "{\n" +
                    "\"name\": \"<the name of the quest>\",\n" +
                    "\"stages\": [\"<\n" +
                    "this field must contain a JSON list containing each quest stage.\n" +
                    "Each individual event stage on this list must have the following json format:" +
                    "{\n" +
                    "\"description\": \"<a very short description of this quest stage.>\",\n" +
                    "\"objective\": \"<the player's objective during this quest stage.>\",\n" +
                    "\"target\": \"<the name of the entity to which the player must perform an action to complete this quest stage.>\",\n" +
                    "\"action\": \"<the name (one word) of the action that the player must perform on the target entity to complete this quest stage.>\",\n" +
                    "}\n" +
                    ">\"]\n" +
                    "}";
                string msg = "";
                msg += details;
                var answer = await AskQuestionToGenerator(rules, msg, null);
                Debug.Log($"Try Generate Quest!\n{answer}");
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

        #endregion
    }
}