using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Entity", menuName = "Sentience/Entity/Type")]
    public sealed class EntityType : ScriptableObject
    {
        public string Name = "Entity";
        public string Description = "";
        public List<Interaction> Interactions = new();
        public List<EntityComponentType> Components = new();

        public void SpawnData(EntityData data, System.Random random)
        {
            data.Type = this;
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            SpawnEditorComponents();
        }

        public void SpawnEditorComponents()
        {
            foreach (var component in Components)
            {
                string n = component.Component.ToString().Split('.')[^1];
                string testName = n + "Authoring";
                if (component.Authoring == null || testName != component.Authoring.ToString().Split('.')[^1])
                {
                    component.Name = n;
                    component.Authoring = component.SpawnAuthoringComponent(component.Component);
                }
            }
        }
#endif
    }
}