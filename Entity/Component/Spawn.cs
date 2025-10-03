using System.Collections.Generic;
using MindTheatre;
using Unity.Rendering;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace Sentience
{
    [System.Serializable]
    public class Spawn : EntityComponent
    {
        public EntitySpawn Prefab;
        public EntitySpawn Spawned;

        public void OnSpawn(System.Random random, Transform parent, Vector3 worldPosition)
        {
            ID id = Data.Get<ID>();

            if (Prefab != null)
            {
                EntitySpawn spawn = Object.Instantiate(Prefab.gameObject, parent).GetComponent<EntitySpawn>();
                Spawned = spawn;

                Spawned.OnSpawn(Data, id.Type, worldPosition);

                id.OnUpdatePosition += (Vector3 pos) => { Spawned.transform.position = pos; };
                id.OnUpdateRotation += (Vector3 rot) => { Spawned.transform.rotation = Quaternion.Euler(rot); };

                if (Data.Has<Body>())
                {
                    Body body = Data.Get<Body>();
                    body.OnSpawn(spawn);
                }
            }
            id.Position = worldPosition;
        }
    }

    [System.Serializable]
    public class SpawnAuthoring : EntityAuthoring
    {
        public List<EntitySpawn> Prefabs = new();

        public override IEntityComponent Spawn(System.Random random)
        {
            Spawn spawn = new();
            if (Prefabs.Count > 0) spawn.Prefab = Prefabs[random.Next(0, Prefabs.Count)];
            return spawn;
        }
    }
}