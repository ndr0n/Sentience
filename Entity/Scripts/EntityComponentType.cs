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
            Attributes,
            Identity,
            Persona,
            Lock,
            Journal,
            Avatar,
            Effects,
            Actions,
            Weapon,
            Equipment,
        }

        [HideInInspector] public string Name;
        public ComponentType Component;
        [SerializeReference] public EntityAuthoring Authoring;

        public IEnumerable<Type> FindDerivedTypes(Assembly assembly, Type baseType)
        {
            return assembly.GetTypes().Where(t => t != baseType && baseType.IsAssignableFrom(t));
        }

        public EntityAuthoring SpawnAuthoringComponent(ComponentType componentType)
        {
            EntityAuthoring component = null;
            Type baseType = typeof(EntityAuthoring);
            List<Type> authoringTypes = FindDerivedTypes(baseType.Assembly, baseType).ToList();
            foreach (var authoringType in authoringTypes)
            {
                if (authoringType.Name.Replace("Authoring", "") == Component.ToString())
                {
                    component = Activator.CreateInstance(authoringType) as EntityAuthoring;
                    break;
                }
            }
            return component;
        }
    }
}