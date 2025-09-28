using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    [System.Serializable]
    public class EntityComponentType
    {
        public enum ComponentType
        {
            Item,
            Body,
            Inventory,
            Health,
            Identity,
            Persona,
            Attributes,
            Avatar,
            Journal,
        }

        [HideInInspector] public string Name;
        public ComponentType Component;
        [SerializeReference] public EntityComponentAuthoring Authoring;

        public IEnumerable<Type> FindDerivedTypes(Assembly assembly, Type baseType)
        {
            return assembly.GetTypes().Where(t => t != baseType && baseType.IsAssignableFrom(t));
        }

        public EntityComponentAuthoring SpawnAuthoringComponent(ComponentType componentType)
        {
            EntityComponentAuthoring component = null;
            Type baseType = typeof(EntityComponentAuthoring);
            List<Type> authoringTypes = FindDerivedTypes(baseType.Assembly, baseType).ToList();
            foreach (var authoringType in authoringTypes)
            {
                if (authoringType.Name.Replace("Authoring", "") == Component.ToString())
                {
                    component = Activator.CreateInstance(authoringType) as EntityComponentAuthoring;
                    break;
                }
            }
            return component;
        }
    }
}