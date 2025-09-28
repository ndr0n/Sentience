using System;
using UnityEngine;

namespace Sentience
{
    public enum ComponentType
    {
        Body,
        Item,
        Inventory,
        ID,
        Persona,
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
            EntityComponent entityComponent = null;
            switch (componentType)
            {
                case ComponentType.Body:
                    entityComponent = new Body();
                    break;
                case ComponentType.Item:
                    entityComponent = new Item();
                    break;
                case ComponentType.Inventory:
                    entityComponent = new Inventory();
                    break;
                case ComponentType.ID:
                    entityComponent = new ID();
                    break;
                case ComponentType.Persona:
                    entityComponent = new Persona();
                    break;
                case ComponentType.Journal:
                    entityComponent = new Journal();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return entityComponent;
        }
    }
}