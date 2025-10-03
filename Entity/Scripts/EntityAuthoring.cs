using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    public abstract class EntityAuthoring
    {
        [HideInInspector] public string Name;
        public abstract IEntityComponent Spawn(System.Random random);
    }
}