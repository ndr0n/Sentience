using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    public abstract class EntityAuthoring
    {
        public abstract IEntityComponent Spawn(System.Random random);
    }
}