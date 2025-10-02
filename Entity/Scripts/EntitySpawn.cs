using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    public abstract class EntitySpawn : MonoBehaviour
    {
        public EntityType Type;
        [SerializeReference] public EntityData Data;

        public void OnSpawn(EntityData data, EntityType type, Vector3 position)
        {
            Data = data;
            Type = type;
            name = data.Name;
            transform.position = position;
            OnSpawn();
        }

        protected abstract void OnSpawn();
    }
}