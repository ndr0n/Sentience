using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    public class EntitySpawn : MonoBehaviour
    {
        public EntityType Type;
        [SerializeReference] public EntityData Data;

        public void SpawnEntity(EntityData data, EntityType type, Vector3 position, System.Random random)
        {
            Data = data;
            Type = type;
            name = data.Name;
            transform.position = position;
            foreach (var c in Data.Components) c.Component.OnSpawn(this);
        }
    }
}