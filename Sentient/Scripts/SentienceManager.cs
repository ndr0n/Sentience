using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LLMUnity;
using Sentience;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Sentience
{
    public class SentienceManager : MonoBehaviour
    {
        public static SentienceManager Instance;
        public DungeonMaster DungeonMaster;
        public ItemDatabase ItemDatabase;
        public FactionDatabase FactionDatabase;
        public SpeciesDatabase SpeciesDatabase;

        [Header("LLM")]
        public bool LLMEnabled;
        public bool RAGEnabled;
        public RagManager RagManager;
        public LLMAgent Character;
        public LLM LLM;
        public LLM RAG;
        string systemPrompt = "";
        bool awaitingResponse = false;
        bool quit = false;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                LLM.gameObject.SetActive(LLMEnabled);
                if (!LLMEnabled) Destroy(LLM.gameObject);
                RAG.gameObject.SetActive(RAGEnabled);
                if (!RAGEnabled) Destroy(RAG.gameObject);
            }
            else Destroy(gameObject);
        }

        void Start()
        {
            if (RAGEnabled) RagManager.Init(ItemDatabase, FactionDatabase, SpeciesDatabase);
            if (LLMEnabled) _ = InitCharacter();
        }

        void OnDestroy()
        {
            quit = true;
            Character.CancelRequests();
            awaitingResponse = false;
            // if (LLM != null)
            // {
            //     LLM.StopAllCoroutines();
            //     Destroy(LLM.gameObject);
            // }
            //
            // if (RAG != null)
            // {
            //     RAG.StopAllCoroutines();
            //     Destroy(RAG.gameObject);
            // }
        }

        #region Character

        readonly string characterRules =
            "You must always answer in less than 30 words.\n" +
            "You must never describe what your character is doing and must only respond with the speech of your character.\n" +
            "I will use text wrapped in [ ] to describe what your character is seeing and what is happening around you so you can have some context for the conversation.\n" +
            "You must never use [ ] and must always answer only with the speech of your character.\n" +
            "You must always answer in character and never break role-play.\n" +
            "You must never say that you are an AI chatbot.\n";

        public async Task InitCharacter()
        {
            if (!LLMEnabled) return;
            awaitingResponse = true;
            Character.systemPrompt = "";
            Character.SetGrammar("");
            systemPrompt =
                $"You are role-playing and impersonating a fictional character in the world of: {DungeonMaster.World}.\n" +
                $"The main lore about the world is: {DungeonMaster.Lore}.\n" +
                $"{characterRules}";
            await Character.Warmup(systemPrompt);
            awaitingResponse = false;
        }

        public async Task<string> AskQuestionFromSentience(Sentient sentient, string message, string details, Action<string> onReply)
        {
            if (!LLMEnabled) return "Sentience is disabled.";
            while (awaitingResponse) await Task.Delay(500);
            if (quit) return null;

            awaitingResponse = true;
            Character.seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
            // Character.systemPrompt = $"{characterRules}\n{sentient.Personality}\n{details}";
            Character.systemPrompt = $"{systemPrompt}\n{sentient.Personality}\n{details}";
            await Character.ClearHistory();
            foreach (var msg in sentient.Messages)
            {
                if (msg.Role == "assistant") await Character.AddAssistantMessage(msg.Content);
                else await Character.AddUserMessage(msg.Content);
            }

            string response = await Character.Chat(message, onReply, null, false);
            awaitingResponse = false;
            return response;
        }

        public async Task<string> AskQuestionCharacterSingle(string personality, string message, Action<string> onReply)
        {
            if (!LLMEnabled) return "Sentience is disabled.";
            while (awaitingResponse) await Task.Delay(250);
            awaitingResponse = true;
            Character.seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
            Character.systemPrompt = $"{systemPrompt}\n{characterRules}\n{personality}";
            await Character.ClearHistory();
            string response = await Character.Chat(message, onReply, null, false);
            awaitingResponse = false;
            return response;
        }

        #endregion
    }
}