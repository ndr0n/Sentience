using System.Collections.Generic;
using Unity.Burst.CompilerServices;

namespace Sentience
{
    [System.Serializable]
    public class CohereMessage
    {   
        public string role = null;
        public string message = null;
    }

    [System.Serializable]
    public class CohereRequest
    {
        public string message = null;
        public List<CohereMessage> chat_history = new();
        public List<object> connectors = new();
    }

    [System.Serializable]
    public static class CohereMessageRole
    {
        public static readonly string System = "SYSTEM";
        public static readonly string User = "USER";
        public static readonly string Chatbot = "CHATBOT";
    }

    [System.Serializable]
    public class CohereResponseModel
    {
        public string response_id;
        public string text;
        public string generation_id;
        public string chat_history;
        public string finish_reason;
        public string meta;
    }
}