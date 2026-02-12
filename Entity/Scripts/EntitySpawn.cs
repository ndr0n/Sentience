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

        public void OnSpawn(EntityData data, EntityType type, Vector3 position)
        {
            Data = data;
            Data.Init(null);
            Type = type;
            name = data.Name;
            transform.position = position;
            foreach (var c in Data.Components) c.Component.OnSpawn(this);
        }
    }
}