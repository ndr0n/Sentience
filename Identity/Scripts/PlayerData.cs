using System.Collections.Generic;
using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    public class PlayerData
    {
        [SerializeReference] public IdentityData Data;
        [SerializeReference] public Journal Journal;

        public PlayerData(IdentityData data, Journal journal)
        {
            Data = data;
            Journal = journal;
        }
    }
}