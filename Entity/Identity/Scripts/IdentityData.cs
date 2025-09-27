using MindTheatre;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace Sentience
{
    [System.Serializable]
    public class IdentityData : EntityData
    {
        public string Location = "";
        public Identity Prefab;
        public Faction Faction;
        [SerializeReference] public Persona Persona = null;
        [SerializeReference] public Inventory Inventory = null;

        [Header("Runtime")]
        public Identity Spawn;
        public Vector3 SpawnPosition;

        public IdentityType IdentityType => Type as IdentityType;

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
            identity.name = Name;
            identity.Type = IdentityType;
            identity.Type.OnSpawnIdentity(identity, random);
        }

        public virtual bool IsHostile(IdentityData id)
        {
            if (Faction == null) return false;
            if (id.Faction == null) return false;
            return Faction.IsHostile(id.Faction);
        }
    }
}