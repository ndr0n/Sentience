using System.Collections.Generic;
using MindTheatre;
using Unity.Rendering;
using Unity.VisualScripting;
using UnityEditor;
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
        public Vector3 SpawnPoint = Vector3.zero;

        public EntitySpawn SpawnEntity(Transform parent, Vector3 worldPosition, Vector3 worldRotation)
        {
            ID id = Data.Get<ID>();

            EntitySpawn spawn = null;
            if (Application.isPlaying) spawn = Object.Instantiate(Prefab.gameObject, parent).GetComponent<EntitySpawn>();
            else spawn = PrefabUtility.InstantiatePrefab(Prefab.gameObject, parent).GetComponent<EntitySpawn>();
            Spawned = spawn;

            // id.OnUpdatePosition += (Vector3 pos) => { Spawned.transform.position = pos; };
            // id.OnUpdateRotation += (Vector3 rot) => { Spawned.transform.rotation = Quaternion.Euler(rot); };

            id.Position = worldPosition;
            id.Rotation = worldRotation;

            SpawnPoint = worldPosition;
            Spawned.OnSpawn(Data, Data.Type, worldPosition, worldRotation);
            
            return spawn;
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