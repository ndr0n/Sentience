using System;
using System.Collections.Generic;
using LLMUnity;
using Sentience;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    public class Sentient : MonoBehaviour
    {
        public int MemorySize = 100;
        public SentienceCharacter Character;
        [HideInInspector] public string Personality;
        [HideInInspector] public List<ChatMessage> Messages = new();

        public void Init(SentienceCharacter character, string details)
        {
            Messages.Clear();
            Character = character;
            Personality = $"Your character name is: {Character.Name}.\n" +
                          $"Your character species is {Character.Species}" +
                          $"Your character description: {Character.Description}.\n" +
                          $"Your character current location is: {Character.Location}.\n";
            foreach (var item in Character.Inventory) Personality += $"You currently have {item} in your inventory.\n";
            Personality += details;
            // Personality += "You must always speak as your character.";
        }

        public async Awaitable AskQuestion(string origin, string message, string details, Callback<string> onReply, Action<string> onFinish)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                Debug.Log($"{origin} asked {Character.Name}:\n{message}");
                AddMessage(origin, message);
                string response = null;
                response = await SentienceManager.Instance.AskQuestionFromSentience(this, message, details, onReply);
                if (response != null) AddMessage("assistant", response);
                // response = TrimName(response, Persona.PersonaData.ID.Name);
                onFinish?.Invoke(response);
            }
            else onFinish?.Invoke(message);
        }

        public void AddMessage(string role, string msg)
        {
            Messages.Add(new ChatMessage() {role = role, content = msg});
            if (Messages.Count > MemorySize) Messages.RemoveAt(0);
        }

        public static string TrimName(string value, string _name)
        {
            return value.Trim().TrimStart($"{_name}: ").TrimStart($"{_name}:").TrimStart($"{_name} : ").TrimStart($"{_name} :").TrimStart($"{_name.Split(" ")[0]}: ").TrimStart($"{_name.Split(" ")[0]}:").TrimStart($"{_name.Split(" ")[0]} : ").TrimStart($"{_name.Split(" ")[0]} :").TrimStart("\"").TrimEnd().TrimEnd("\"");
        }
    }
}