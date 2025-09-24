using System;
using System.Collections.Generic;
using LLMUnity;
using MindTheatre;
using Sentience;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    public class Sentient : MonoBehaviour
    {
        public Identity Identity;
        public int MemorySize = 100;
        [HideInInspector] public string Personality;
        [HideInInspector] public List<ChatMessage> Messages = new();

        public void Init()
        {
            Messages.Clear();
            Personality = $"Your character name is: {Identity.Data.Name}.\n" +
                          $"Your character species is {Identity.Data.Persona.Species}" +
                          $"Your character description: {Identity.Data.Description}.\n" +
                          $"Your character current location is: {Identity.Data.Location}.\n";
            if (Identity.Data.Faction != null) Personality += $"Your Faction is {Identity.Data.Faction}\n";
            foreach (var item in Identity.Data.Inventory.Items) Personality += $"You currently have {item} in your inventory.\n";
            // Personality += "You must always speak as your character.";
        }

        public async Awaitable AskQuestion(string origin, string message, string details, Callback<string> onReply, Action<string> onFinish)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                Debug.Log($"{origin} asked {Identity.Data.Name}:\n{message}");
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