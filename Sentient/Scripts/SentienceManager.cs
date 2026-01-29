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

        [Header("LLM")] public bool LLMEnabled;
        public bool RAGEnabled;
        public RagManager RagManager;
        public LLMAgent Character;
        public LLM LLM;
        public LLM RAG;
        string systemPrompt = "";

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
            if (LLM != null)
            {
                LLM.StopAllCoroutines();
                Destroy(LLM.gameObject);
            }

            if (RAG != null)
            {
                RAG.StopAllCoroutines();
                Destroy(RAG.gameObject);
            }
        }

        #region Character

        readonly string characterRules =
            "You must never describe what your character is doing and must only respond with the speech of your character.\n" +
            "You must always answer in less than 30 words.\n" +
            "You must always answer in character and never break role-play.\n" +
            "You must only write the speech of your own character.\n" +
            "You must never say that you are an AI chatbot.\n";

        public async Awaitable InitCharacter()
        {
            if (!LLMEnabled) return;
            awaitingResponse = true;
            Character.systemPrompt = "";
            Character.SetGrammar("");
            systemPrompt =
                $"You are role-playing and impersonating a fictional character in the world of: {DungeonMaster.World}.\n" +
                $"The main lore about the world is: {DungeonMaster.Lore}.\n";
            await Character.Warmup(systemPrompt);
            awaitingResponse = false;
        }

        bool awaitingResponse = false;

        public async Awaitable<string> AskQuestionFromSentience(Sentient sentient, string message, string details,
            Action<string> onReply)
        {
            if (!LLMEnabled) return "Sentience is disabled.";
            while (awaitingResponse) await Awaitable.WaitForSecondsAsync(0.25f);
            awaitingResponse = true;
            Character.systemPrompt = $"{characterRules}\n{sentient.Personality}\n{details}";
            await Character.ClearHistory();
            foreach (var msg in sentient.Messages)
            {
                if (msg.role == "assistant") await Character.AddAssistantMessage(msg.content);
                else await Character.AddUserMessage(msg.content);
            }

            string response = await Character.Chat(message, onReply, null, false);
            awaitingResponse = false;
            return response;
        }

        public async Awaitable<string> AskQuestionCharacterSingle(string personality, string message,
            Action<string> onReply)
        {
            if (!LLMEnabled) return "Sentience is disabled.";
            while (awaitingResponse) await Awaitable.WaitForSecondsAsync(0.25f);
            awaitingResponse = true;
            Character.systemPrompt = $"{characterRules}\n{personality}";
            await Character.ClearHistory();
            string response = await Character.Chat(message, onReply, null, false);
            awaitingResponse = false;
            return response;
        }

        #endregion
    }
}