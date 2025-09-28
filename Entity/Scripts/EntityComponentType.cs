using System;
using UnityEngine;

namespace Sentience
{
    public enum ComponentType
    {
        Item,
        Body,
        Inventory,
        ID,
        Persona,
        Attributes,
        Journal,
    }

    [System.Serializable]
    public class EntityComponentType
    {
        [HideInInspector] public string Name;
        public ComponentType Type;
        [SerializeReference] public EntityComponent Component;

        public EntityComponent SpawnComponent(ComponentType componentType)
        {
            EntityComponent entityComponent = componentType switch
            {
                ComponentType.Body => new Body(),
                ComponentType.Item => new Item(),
                ComponentType.Inventory => new Inventory(),
                ComponentType.ID => new ID(),
                ComponentType.Persona => new Persona(),
                ComponentType.Attributes => new Attributes(),
                ComponentType.Journal => new Journal(),
                _ => throw new ArgumentOutOfRangeException()
            };
            return entityComponent;
        }
    }
}