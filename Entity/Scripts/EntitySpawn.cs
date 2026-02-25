using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    public class EntitySpawn : MonoBehaviour
    {
        public EntityType Type;
        [SerializeReference] public EntityData Data;

        // void Awake()
        // {
        //     if (Data != null) Data.Init(null);
        // }

        public void OnSpawn(EntityData data, EntityType type, Vector3 worldPosition, Vector3 worldRotation)
        {
            Data = data;
            Type = type;
            name = data.Name;
            Spawn spawn = data.Get<Spawn>();
            spawn.Spawned = this;
            spawn.SpawnPoint = worldPosition;
            data.ID.Position = worldPosition;
            data.ID.Rotation = worldRotation;
            transform.position = worldPosition;
            transform.eulerAngles = worldRotation;
            foreach (var c in Data.Components) c.Component.OnSpawn(this);
        }
    }
}