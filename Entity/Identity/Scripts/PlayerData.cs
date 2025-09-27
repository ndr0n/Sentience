using System.Collections.Generic;
using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    public class PlayerData : IdentityData
    {
        public Journal Journal = new();

        public static PlayerData Create(IdentityType identityType, System.Random random, Vector3 position, string location, Journal journal)
        {
            PlayerData pd = new PlayerData()
            {
                Type = identityType,
                Name = identityType.Name,
                Location = location,
                Description = identityType.Description,
                Prefab = identityType.Prefab[random.Next(identityType.Prefab.Count)],
                SpawnPosition = position,
                Journal = journal
            };
            if (identityType.HasInventory) pd.Inventory = new();
            return pd;
        }
    }
}