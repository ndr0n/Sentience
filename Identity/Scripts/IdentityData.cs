using MindTheatre;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

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
        public Faction Faction;
        [SerializeReference] public Persona Persona = null;
        [SerializeReference] public Inventory Inventory = null;

        [Header("Runtime")]
        public Identity Spawn;
        public Vector3 SpawnPosition;

        public static IdentityData Create(IdentityType identityType, System.Random random, Vector3 position, string location)
        {
            IdentityData id = new IdentityData()
            {
                Type = identityType,
                Name = identityType.Name,
                Location = location,
                Description = identityType.Description,
                Prefab = identityType.Prefab[random.Next(identityType.Prefab.Count)],
                SpawnPosition = position,
            };
            if (identityType.HasInventory) id.Inventory = new();
            return id;
        }

        public static Identity SpawnIdentity(IdentityData data, System.Random random, Transform parent)
        {
            Identity spawn = Object.Instantiate(data.Prefab.gameObject, parent).GetComponent<Identity>();
            spawn.transform.position = data.SpawnPosition;
            data.Init(spawn, random);
            return spawn;
        }

        public void Init(Identity identity, System.Random random)
        {
            Spawn = identity;
            identity.Data = this;
            identity.Type = Type;
            identity.name = Name;
            identity.Type.OnSpawnIdentity(identity, random);
        }
    }
}