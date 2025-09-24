using Unity.VisualScripting;
using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    public class IdentityData
    {
        public string Name = "Name";
        public string Description = "Description";
        public Identity Prefab;
        public Inventory Inventory = new();
        public Identity Spawn;
        public Vector3 SpawnPosition;

        public static IdentityData Create(IdentityType identityType, System.Random random)
        {
            // IdentityData id = CreateInstance<IdentityData>();
            IdentityData id = new();
            id.Name = identityType.Name;
            id.Description = identityType.Description;
            id.Prefab = identityType.Prefab[random.Next(identityType.Prefab.Count)];
            id.Inventory = new();
            return id;
        }

        public static Identity SpawnIdentity(IdentityData data, IdentityType type, System.Random random, Vector3 position, Transform parent)
        {
            Identity spawn = Object.Instantiate(data.Prefab.gameObject, parent).GetComponent<Identity>();
            spawn.transform.position = position;
            data.SpawnPosition = position;
            data.Init(spawn, type, random);
            return spawn;
        }

        public void Init(Identity identity, IdentityType type, System.Random random)
        {
            Spawn = identity;
            identity.Data = this;
            identity.Type = type;
            identity.Type.OnSpawnIdentity(identity, random);
        }
    }
}