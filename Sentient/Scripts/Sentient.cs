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
        public int MemorySize = 100;
        [HideInInspector] public string Personality;
        [HideInInspector] public List<ChatMessage> Messages = new();
        Identity identity;

        public void Init(Identity _identity)
        {
            identity = _identity;
            ID id = identity.Data.Get<ID>();
            Messages.Clear();
            Personality = $"Your character name is: {id.Name}.\n" +
                          $"Your character species is {identity.Species.Name}" +
                          $"Your character description: {id.Description}.\n" +
                          $"Your character current location is: {identity.Location}.\n";
            if (identity.Faction != null) Personality += $"Your Faction is {identity.Faction.Name}\n";
            Inventory inventory = identity.Data.Get<Inventory>();
            foreach (var item in inventory.Items) Personality += $"You currently have {item.Name} in your inventory.\n";
            Personality += "You must always speak as your character.";
        }

        public async Awaitable AskQuestion(string origin, string message, string details, Action<string> onReply,
            Action<string> onFinish)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(message))
                {
                    ID id = identity.Data.Get<ID>();
                    Debug.Log($"{origin} asked {id.Name}:\n{message}");
                    AddMessage(origin, message);
                    string response = null;
                    response = await SentienceManager.Instance.AskQuestionFromSentience(this, message, details,
                        (r) => { onReply?.Invoke(r); });
                    if (response != null) AddMessage("assistant", response);
                    // response = TrimName(response, Persona.PersonaData.ID.Name);
                    onFinish?.Invoke(response);
                }
                else onFinish?.Invoke(message);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public void AddMessage(string role, string msg)
        {
            Messages.Add(new ChatMessage(role, msg));
            if (Messages.Count > MemorySize) Messages.RemoveAt(0);
        }

        public static string TrimName(string value, string _name)
        {
            return value.Trim().TrimStart($"{_name}: ").TrimStart($"{_name}:").TrimStart($"{_name} : ")
                .TrimStart($"{_name} :").TrimStart($"{_name.Split(" ")[0]}: ").TrimStart($"{_name.Split(" ")[0]}:")
                .TrimStart($"{_name.Split(" ")[0]} : ").TrimStart($"{_name.Split(" ")[0]} :").TrimStart("\"").TrimEnd()
                .TrimEnd("\"");
        }
    }
}