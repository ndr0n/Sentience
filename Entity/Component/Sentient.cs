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
    [System.Serializable]
    public struct SentientMessage
    {
        public string Role;
        public string Content;

        public SentientMessage(string role, string content)
        {
            Role = role;
            Content = content;
        }
    }

    [System.Serializable]
    public class Sentient : EntityComponent
    {
        [Multiline]
        public string Personality = "";

        public int MemorySize = 10;
        public List<SentientMessage> Messages = new();

        public override void OnInit(EntityData data, System.Random random)
        {
            base.OnInit(data, random);
            Messages.Clear();
            Identity identity = Data.TryGet<Identity>();
            if (identity != null) InitIdentity(identity);
        }

        public void InitIdentity(Identity identity)
        {
            ID id = identity.Data.Get<ID>();
            Personality = $"Your character name is: {id.Name}.\n" +
                          $"Your character species is: {identity.Species.Name}\n" +
                          $"Your character description: {id.Description}.\n" +
                          $"Your character current location is: {identity.Location}.\n";
            if (identity.Faction != null) Personality += $"Your Faction is: {identity.Faction.Name}\n";
            Inventory inventory = identity.Data.Get<Inventory>();
            foreach (var item in inventory.Items) Personality += $"You currently have {item.Item.Data.Name} in your inventory.\n";
            Personality += "You must always speak as your character.\n";
        }

        public async Awaitable AskQuestion(string origin, string message, string details, Action<string> onReply, Action<string> onFinish)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(message))
                {
                    ID id = Data.Get<ID>();
                    Debug.Log($"{origin} asked {id.Name}:\n{message}");
                    AddMessage(origin, message);
                    string response = null;
                    response = await SentienceManager.Instance.AskQuestionFromSentience(this, message, details, (r) => { onReply?.Invoke(r); });
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
            Messages.Add(new SentientMessage(role, msg));
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

    [System.Serializable]
    public class SentientAuthoring : EntityAuthoring
    {
        [Multiline]
        public string Personality = "";

        public int MemorySize = 10;

        public override IEntityComponent Spawn(System.Random random)
        {
            Sentient sentient = new();
            sentient.MemorySize = MemorySize;
            sentient.Personality = Personality;
            return sentient;
        }
    }
}