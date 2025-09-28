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

        public void Init(ItemDatabase itemDatabase, FactionDatabase factionDatabase, SpeciesDatabase speciesDatabase)
        {
            loadingRag = null;
            Rag.Clear();
            RagSingle.Clear();
            loadingRag = CreateEmbeddings(itemDatabase, factionDatabase, speciesDatabase);
        }

        public async Awaitable CreateEmbeddings(ItemDatabase itemDatabase, FactionDatabase factionDatabase, SpeciesDatabase speciesDatabase)
        {
            foreach (var item in itemDatabase.Items) await Rag.Add($"{item.Name}|{item.GetType().ToString().Split(".")[^1]}", "Item");
            foreach (var faction in factionDatabase.Faction) await Rag.Add($"{faction.Name}|{faction.Description}", "Faction");
            foreach (var species in speciesDatabase.Species) await Rag.Add($"{species.Name}|{species.Description}", "Species");
            loadingRag = null;
        }

        public async Awaitable<EntityType> GetMostSimilarItem(ItemDatabase itemDatabase, string itemDescription)
        {
            if (loadingRag != null) await loadingRag;
            (string[] similar, float[] distances) = await Rag.Search(itemDescription, 1, "Item");
            string itemName = similar[0].Split("|")[0];
            return itemDatabase.GetItem(itemName);
        }

        public async Awaitable<Faction> GetMostSimilarFaction(FactionDatabase factionDatabase, string factionDescription)
        {
            if (loadingRag != null) await loadingRag;
            (string[] similar, float[] distances) = await Rag.Search(factionDescription, 1, "Faction");
            string factionName = similar[0].Split("|")[0];
            return factionDatabase.GetFaction(factionName);
        }

        public async Awaitable<Species> GetMostSimilarSpecies(SpeciesDatabase speciesDatabase, string speciesDescription)
        {
            if (loadingRag != null) await loadingRag;
            (string[] similar, float[] distances) = await Rag.Search(speciesDescription, 1, "Species");
            string factionName = similar[0].Split("|")[0];
            return speciesDatabase.GetFaction(factionName);
        }

        public async Awaitable<string> GetMostSimilar(List<string> options, string description)
        {
            RagSingle.Clear();
            foreach (var option in options) await RagSingle.Add(option);
            (string[] similar, float[] distances) = await RagSingle.Search(description, 1);
            return similar[0];
        }
    }
}