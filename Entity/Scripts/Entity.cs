using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    public abstract class Entity : MonoBehaviour
    {
        public EntityType Type;
        [SerializeReference] public EntityData Data = null;

        public void Spawn(EntityType type, EntityData data, Vector3 position)
        {
            Type = type;
            Data = data;
            name = data.Name;
            transform.position = position;
            OnSpawn();
        }

        protected abstract void OnSpawn();
    }
}