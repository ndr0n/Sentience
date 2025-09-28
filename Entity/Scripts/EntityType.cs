using System.Collections.Generic;
using UnityEngine;

namespace Sentience
{
    public abstract class EntityType : ScriptableObject
    {
        public List<Entity> Prefab;
        public string Name = "Entity";
        public string Description = "";
        public List<Interaction> Interactions = new();

        public void SpawnData(EntityData data, System.Random random)
        {
            data.Type = this;
            if (Prefab.Count > 0)
            {
                Body body = new Body(data, this, random);
                data.Components.Add(new(body));
                Debug.Log($"Added body to {data.Name}");
            }
            OnSpawnData(data, random);
        }

        protected abstract void OnSpawnData(EntityData data, System.Random random);

        public void SpawnEntity(Entity entity, System.Random random)
        {
            OnSpawnEntity(entity, random);
        }

        protected abstract void OnSpawnEntity(Entity entity, System.Random random);
    }
}