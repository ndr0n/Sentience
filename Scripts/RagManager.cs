using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using LLMUnity;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

namespace Sentience
{
    public class RagManager : MonoBehaviour
    {
        public static RagManager Instance;
        public RAG Rag;
        public RAG RagSingle;
        Awaitable loadingRag = null;

        void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void Init(ItemData itemData)
        {
            loadingRag = null;
            Rag.Clear();
            RagSingle.Clear();
            loadingRag = CreateEmbeddings(itemData);
        }

        public async Awaitable CreateEmbeddings(ItemData itemData)
        {
            foreach (var item in itemData.Items) await Rag.Add($"{item.Name}|{item.GetType().ToString().Split(".")[^1]}", "Item");
            // foreach (var faction in Settings.Instance.Data.FactionData.Faction) await Rag.Add($"{faction.Name}|{faction.Description}", "Faction");
            loadingRag = null;
        }

        public async Awaitable<Item> GetMostSimilarItem(ItemData itemData, string itemDescription)
        {
            if (loadingRag != null) await loadingRag;
            (string[] similar, float[] distances) = await Rag.Search(itemDescription, 1, "Item");
            string itemName = similar[0].Split("|")[0];
            return itemData.GetItem(itemName);
        }

        // public async Awaitable<Faction.Faction> GetMostSimilarFaction(string factionDescription)
        // {
        // if (loadingRag != null) await loadingRag;
        // (string[] similar, float[] distances) = await Rag.Search(factionDescription, 1, "Faction");
        // string factionName = similar[0].Split("|")[0];
        // return Settings.Instance.Data.FactionData.GetFaction(factionName);
        // }

        public async Awaitable<string> GetMostSimilar(List<string> options, string description)
        {
            RagSingle.Clear();
            foreach (var option in options) await RagSingle.Add(option);
            (string[] similar, float[] distances) = await RagSingle.Search(description, 1);
            return similar[0];
        }
    }
}