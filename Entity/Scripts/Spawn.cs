using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    public abstract class Spawn : MonoBehaviour
    {
        public Entity Entity;
        public EntityType Type;

        public void OnSpawn(Entity entity, EntityType type, Vector3 position)
        {
            Entity = entity;
            Type = type;
            Info info = EntityLibrary.Get<Info>(entity);
            name = info.Name;
            transform.position = position;
            OnSpawn();
        }

        protected abstract void OnSpawn();
    }
}