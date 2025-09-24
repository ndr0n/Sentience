using System.Collections.Generic;
using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    public abstract class IdentityType : ScriptableObject
    {
        public List<Identity> Prefab;
        public string Name = "Identity";
        public string Description = "";
        public List<IdentityInteraction> Interactions = new();
        public abstract void OnSpawnIdentity(Identity identity, System.Random random);
    }
}