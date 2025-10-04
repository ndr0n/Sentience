using System;
using System.Collections.Generic;
using System.Linq;
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
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(EntityType)), CanEditMultipleObjects]
    public class EntityType_Editor : Editor
    {
        int selected = 0;
        Type[] types = EntityLibrary.FindDerivedTypes(typeof(EntityAuthoring).Assembly, typeof(EntityAuthoring)).ToArray();
        string[] typeNames = EntityLibrary.GetDerivedTypeNames(typeof(EntityAuthoring).Assembly, typeof(EntityAuthoring), "Authoring").ToArray();

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EntityType entityType = (EntityType) target;
            string[] names = typeNames.Where(x => !entityType.Components.Exists(y => y.Authoring.GetType().ToString().Split('.')[^1].Replace("Authoring", "") == x)).ToArray();
            if (names.Length > 0)
            {
                selected = EditorGUILayout.Popup("Component", selected, names);
                if (GUILayout.Button("Add Component"))
                {
                    foreach (var type in types)
                    {
                        if (type.Name.Split('.')[^1].Replace("Authoring", "") == names[selected])
                        {
                            var component = Activator.CreateInstance(type) as EntityAuthoring;
                            entityType.Components.Add(new(names[selected], component));
                            selected = 0;
                        }
                    }
                }
            }
        }
    }
#endif
}