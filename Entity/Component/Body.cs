using System.Collections.Generic;
using System.Numerics;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;
using Vector2 = System.Numerics.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Sentience
{
    [System.Serializable]
    public class Body : IEntityComponent
    {
        public Entity Prefab;
        public Vector3 Position;
        public Entity Entity;

        EntityData _data;
        public EntityData Data => _data;

        public void Init(EntityData data, Random random)
        {
            _data = data;
        }

        public Entity SpawnBody(Random random, Transform parent, Vector3 worldPosition)
        {
            Entity entity = Object.Instantiate(Prefab.gameObject, parent).GetComponent<Entity>();
            Entity = entity;
            Position = worldPosition;
            Entity.Spawn(Data.Type, Data, Position);
            return entity;
        }
    }

    [System.Serializable]
    public class BodyAuthoring : EntityComponentAuthoring
    {
        public List<Entity> Prefabs = new();

        public override IEntityComponent Spawn(Random random)
        {
            Body body = new();
            body.Prefab = Prefabs[random.Next(0, Prefabs.Count)];
            return body;
        }
    }
}