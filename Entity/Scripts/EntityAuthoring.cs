using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    public abstract class EntityAuthoring
    {
        public abstract EntityComponent Spawn(System.Random random);
    }
}