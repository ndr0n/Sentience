using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using System;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;

namespace Sentience
{
    [System.Serializable]
    public struct ChatApiKey
    {
        public string email;
        public string key;
    }

    public class CohereApi : MonoBehaviour
    {
        public static CohereApi Instance;
        public string URL = "https://api.cohere.ai";
        public string ApiEndpoint = "v1/chat";
        public int CurrentChatApiKey = 0;
#if UNITY_EDITOR
        [Multiline] public string Rules = "You can never say that you are an AI chatbot, an NPC or a non-player character.";
        public List<ChatApiKey> ChatApiKey = new();
#endif
        HttpClient httpClient;

        void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        void Start()
        {
#if UNITY_EDITOR
            Rules = Rules.Replace("\n", "");
            httpClient = new() {BaseAddress = new Uri(URL)};
            httpClient.DefaultRequestHeaders.Add("accept", "application/json");
            SetApiKey(ChatApiKey[CurrentChatApiKey].key);
#endif
        }

        void SetApiKey(string key)
        {
#if UNITY_EDITOR
            if (httpClient.DefaultRequestHeaders.Contains("Authorization")) httpClient.DefaultRequestHeaders.Remove("Authorization");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"bearer {key}");
#endif
        }

        public async Awaitable<string> AskQuestion(string personality, string message, List<CohereMessage> previousMessages = null, bool debug = true)
        {
            if (debug) Debug.Log($"Cohere> Question:\n {message}");
            string response = await AskQuestionRequest(personality, message, previousMessages);
            if (debug) Debug.Log($"Cohere> Response:\n {response}");
            return response;
        }

        async Awaitable<string> AskQuestionRequest(string personality, string message, List<CohereMessage> previousMessages = null, bool debug = false)
        {
#if UNITY_EDITOR
            string extraRules = $"";
            CohereRequest request = new()
            {
                message = message,
                chat_history = new() {new CohereMessage() {role = CohereMessageRole.System, message = $"You are in {DungeonMaster.Instance.World}\n{personality}\n{extraRules}\n{Rules}"}},
            };
            if (previousMessages != null)
            {
                foreach (CohereMessage previousMessage in previousMessages)
                {
                    request.chat_history.Add(previousMessage);
                }
            }
            string postData = JsonUtility.ToJson(request);
            if (debug) Debug.Log(postData);
            using StringContent jsonData = new(postData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync(ApiEndpoint, jsonData);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                string answer = JsonUtility.FromJson<CohereResponseModel>(result).text;
                return answer;
            }
            else
            {
                Debug.Log($"{(int) response.StatusCode} - {response.ReasonPhrase}");
                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    CurrentChatApiKey = ((CurrentChatApiKey + 1) % ChatApiKey.Count);
                    SetApiKey(ChatApiKey[CurrentChatApiKey].key);
                    await Awaitable.WaitForSecondsAsync(1);
                    return await AskQuestionRequest(personality, message, previousMessages);
                }
                return null;
            }
#endif
            return null;
        }
    }
}