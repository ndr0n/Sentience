using System.Collections.Generic;
using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    public abstract class IdentityType : EntityType
    {
        public List<Identity> Prefab;
        public bool HasInventory = true;
        public abstract void OnSpawnIdentity(Identity identity, System.Random random);
    }
}