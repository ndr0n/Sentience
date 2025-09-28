using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Entity", menuName = "Sentience/Entity/Type")]
    public class EntityType : ScriptableObject
    {
        public string Name = "Entity";
        public string Description = "";
        public List<Entity> Prefab;
        public List<Interaction> Interactions = new();
        public List<EntityComponentType> Components = new();

#if UNITY_EDITOR
        void OnValidate()
        {
            SpawnEditorComponents();
        }
#endif

        public void SpawnData(EntityData data, System.Random random)
        {
            data.Type = this;
            OnSpawnData(data, random);
        }

        protected virtual void OnSpawnData(EntityData data, System.Random random)
        {
        }

        public void SpawnEntity(Entity entity, System.Random random)
        {
            OnSpawnEntity(entity, random);
        }

        protected virtual void OnSpawnEntity(Entity entity, System.Random random)
        {
        }

        public void SpawnEditorComponents()
        {
            foreach (var component in Components)
            {
                string n = component.Type.ToString().Split('.')[^1];
                if (n != component.Component.ToString().Split('.')[^1])
                {
                    component.Name = n;
                    component.Component = component.SpawnComponent(component.Type);
                }
            }
        }
    }
}