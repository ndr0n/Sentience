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
        public ItemData ItemData;

        [Header("LLM")]
        public bool LLMEnabled;
        public bool RAGEnabled;
        public RagManager RagManager;
        public LLMCharacter Character;
        public LLM LLM;
        public LLM RAG;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                LLM.gameObject.SetActive(LLMEnabled);
                InitCharacter();
                RAG.gameObject.SetActive(RAGEnabled);
                RagManager.Init(ItemData);
            }
            else Destroy(gameObject);
        }

        void OnDestroy()
        {
            LLM.StopAllCoroutines();
            Destroy(LLM.gameObject);
            RAG.StopAllCoroutines();
            Destroy(RAG.gameObject);
        }

        #region Character

        readonly string characterRules =
            "You must never describe what your character is doing and must only respond with the speech of your character.\n" +
            "You must always answer in less than 30 words.\n" +
            "You must always answer in character and never break role-play.\n" +
            "You must only write the speech of your own character.\n" +
            "You must never say that you are an AI chatbot.\n";

        public void InitCharacter()
        {
            Character.seed = Random.Range(int.MinValue, int.MaxValue);
            Character.cachePrompt = true;
            Character.prompt = "";
            Character.grammarString = "";
            string systemPrompt = $"You are role-playing and impersonating a fictional character in the world of: {DungeonMaster.World}.\n" +
                                  $"The main lore about the world is: {DungeonMaster.Lore}.\n";
            Character.Warmup(systemPrompt);
        }

        bool awaitingResponse = false;

        public async Awaitable<string> AskQuestionFromSentience(Sentient sentient, string message, string details, Callback<string> onReply)
        {
            while (awaitingResponse) await Awaitable.WaitForSecondsAsync(0.25f);
            awaitingResponse = true;
            Character.grammarString = "";
            Character.seed = Random.Range(int.MinValue, int.MaxValue);
            Character.SetPrompt($"{characterRules}\n{sentient.Personality}\n{details}", true);
            foreach (var msg in sentient.Messages) Character.AddMessage(msg.role, msg.content);
            string response = await Character.Chat(message, onReply, null, false);
            awaitingResponse = false;
            return response;
        }

        public async Awaitable<string> AskQuestionCharacterSingle(string personality, string message, Callback<string> onReply)
        {
            while (awaitingResponse) await Awaitable.WaitForSecondsAsync(0.25f);
            awaitingResponse = true;
            Character.grammarString = "";
            Character.seed = Random.Range(int.MinValue, int.MaxValue);
            Character.SetPrompt($"{characterRules}\n{personality}", true);
            string response = await Character.Chat(message, onReply, null, false);
            awaitingResponse = false;
            return response;
        }

        #endregion
    }
}