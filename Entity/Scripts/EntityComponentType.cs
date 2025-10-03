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
        // public enum ComponentType
        // {
        //     Item,
        //     Spawn,
        //     Inventory,
        //     Health,
        //     Attributes,
        //     Identity,
        //     Persona,
        //     Lock,
        //     Journal,
        //     Effects,
        //     Weapon,
        //     Equipment,
        //     Explosive,
        //     Body,
        //     Pawn,
        //     Npc,
        //     Player,
        // }

        // public ComponentType Component;
        [HideInInspector] public string Name;
        [SerializeReference] public EntityAuthoring Authoring;

        // public EntityAuthoring SpawnAuthoringComponent(ComponentType componentType)
        // {
        //     EntityAuthoring component = null;
        //     Type baseType = typeof(EntityAuthoring);
        //     List<Type> authoringTypes = EntityLibrary.FindDerivedTypes(baseType.Assembly, baseType).ToList();
        //     foreach (var authoringType in authoringTypes)
        //     {
        //         if (authoringType.Name.Split('.')[^1].Replace("Authoring", "") == Component.ToString())
        //         {
        //             component = Activator.CreateInstance(authoringType) as EntityAuthoring;
        //             break;
        //         }
        //     }
        //     return component;
        // }

        public EntityComponentType(string name, EntityAuthoring authoring)
        {
            Name = name;
            Authoring = authoring;
        }
    }
}