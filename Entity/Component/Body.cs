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
    public class Body : EntityComponent
    {
        public Spawn Prefab;
        public Vector3 Position;
        public Spawn Spawn;

        public Spawn SpawnBody(Random random, Transform parent, Vector3 worldPosition)
        {
            Spawn spawn = Object.Instantiate(Prefab.gameObject, parent).GetComponent<Spawn>();
            Spawn = spawn;

            Info info = EntityLibrary.Get<Info>(Entity);
            Spawn.OnSpawn(Entity, info.Type, worldPosition);
            return spawn;
        }
    }

    [System.Serializable]
    public class BodyAuthoring : EntityAuthoring
    {
        public List<Spawn> Prefabs = new();

        public override IComponentData Spawn(Random random)
        {
            Body body = new();
            body.Prefab = Prefabs[random.Next(0, Prefabs.Count)];
            return body;
        }
    }
}