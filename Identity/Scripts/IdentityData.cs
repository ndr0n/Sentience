using Unity.VisualScripting;
using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    public class IdentityData
    {
        public string Name = "Name";
        public string Location = "";
        public string Description = "Description";
        public IdentityType Type;
        public Identity Prefab;
        public Inventory Inventory;

        [Header("Runtime")]
        public Identity Spawn;
        public Vector3 SpawnPosition;

        public static IdentityData Create(IdentityType identityType, System.Random random, Vector3 position, string location)
        {
            // IdentityData id = CreateInstance<IdentityData>();
            IdentityData id = new();
            id.Type = identityType;
            id.Name = identityType.Name;
            id.Location = location;
            id.Description = identityType.Description;
            id.Prefab = identityType.Prefab[random.Next(identityType.Prefab.Count)];
            id.Inventory = new();
            id.SpawnPosition = position;
            return id;
        }

        public static Identity SpawnIdentity(IdentityData data, IdentityType type, System.Random random, Transform parent)
        {
            Identity spawn = Object.Instantiate(data.Prefab.gameObject, parent).GetComponent<Identity>();
            spawn.transform.position = data.SpawnPosition;
            data.Init(spawn, type, random);
            return spawn;
        }

        public void Init(Identity identity, IdentityType type, System.Random random)
        {
            Type = type;
            Spawn = identity;
            identity.Data = this;
            identity.Type = type;
            identity.Type.OnSpawnIdentity(identity, random);
        }
    }
}