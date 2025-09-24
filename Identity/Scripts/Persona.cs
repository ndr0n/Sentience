using System;
using System.Collections.Generic;
using System.Linq;
using DND;
using MindTheatre;
using Sentience;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Sentience
{
    [System.Serializable]
    public class Persona
    {
        public string Species = "";
        public string Desire = "";

        public static async Awaitable<Persona> Generate(IdentityData id, SentienceCharacter character)
        {
            Persona persona = new();
            id.Name = character.Name;
            id.Location = character.Location;
            id.Description = character.Description;
            id.Faction = character.Faction;
            persona.Species = character.Species;
            foreach (var item in character.Inventory)
            {
                id.Inventory.Add(new Item(item, $"Belongs to {character.Name}", 1, await SentienceManager.Instance.RagManager.GetMostSimilarItem(SentienceManager.Instance.ItemData, item)));
            }
            id.Persona = persona;
            return persona;
        }
    }
}