using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    public class Body : EntityComponent
    {
        public Spawn Prefab;
        public Spawn Spawn;

        public Spawn SpawnBody(System.Random random, Transform parent, Vector3 worldPosition)
        {
            Spawn spawn = Object.Instantiate(Prefab.gameObject, parent).GetComponent<Spawn>();
            Spawn = spawn;

            // World.DefaultGameObjectInjectionWorld.EntityManager.AddComponent<LocalTransform>(Data.Entity);

            ID id = Data.Get<ID>();
            id.Position = worldPosition;
            Spawn.OnSpawn(Data, id.Type, worldPosition);

            return spawn;
        }
    }

    [System.Serializable]
    public class BodyAuthoring : EntityAuthoring
    {
        public List<Spawn> Prefabs = new();

        public override IComponentData Spawn(System.Random random)
        {
            Body body = new();
            body.Prefab = Prefabs[random.Next(0, Prefabs.Count)];
            return body;
        }
    }
}