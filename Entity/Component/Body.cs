using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    public class Body : Component
    {
        public Entity Prefab;
        public Entity Entity;
        public Vector3 Position;

        public Body(EntityData data, EntityType spawnType, System.Random random) : base(data)
        {
            _data = data;
            Prefab = spawnType.Prefab[random.Next(spawnType.Prefab.Count)];
        }

        public Entity SpawnBody(System.Random random, Transform parent, Vector3 position)
        {
            Position = position;
            Entity entity = Object.Instantiate(Prefab.gameObject, parent).GetComponent<Entity>();
            entity.Data = Data;
            entity.transform.position = Position;
            Init(entity, random);
            return entity;
        }

        public void Init(Entity entity, System.Random random)
        {
            Entity = entity;
            Entity.name = Data.Name;
            Entity.Type = Data.Type;
            Entity.Type.SpawnEntity(Entity, random);
        }
    }
}