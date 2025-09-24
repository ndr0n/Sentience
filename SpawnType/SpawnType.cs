using System.Collections.Generic;
using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    public abstract class SpawnType : ScriptableObject
    {
        public List<GameObject> Prefab;
        public string Name = "Spawn";
        public string Description = "";

        public virtual void Init(GameObject obj, System.Random random)
        {
            obj.name = Name;
        }
    }
}