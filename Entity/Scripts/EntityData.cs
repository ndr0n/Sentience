using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    [System.Serializable]
    public class EntityComponentData
    {
        [HideInInspector] public string Name;
        [SerializeReference] public EntityComponent EntityComponent;
        [HideInInspector] public int Hash;

        public EntityComponentData(EntityComponent entityComponent)
        {
            string type = entityComponent.GetType().ToString();
            Name = type.Split('.')[^1];
            Hash = type.GetHashCode();
            EntityComponent = entityComponent;
        }
    }

    [System.Serializable]
    public class EntityData
    {
        public string Name;
        public string Description;
        public EntityType Type;
        [SerializeReference] List<EntityComponentData> components = new();

        public EntityData(string name, string description, EntityType type, System.Random random)
        {
            Name = name;
            Description = description;
            Type = type;
            components = new();
            foreach (var componentType in type.Components)
            {
                EntityComponent component = componentType.SpawnComponent(componentType.Type);
                component.Init(this, random);
                components.Add(new(component));
            }
            type.SpawnData(this, random);
        }

        public T Get<T>() where T : EntityComponent
        {
            // int hash = typeof(T).GetHashCode();
            foreach (var c in components)
            {
                if (c.EntityComponent is T t) return t;
            }
            return null;
        }

        public virtual bool IsHostile(EntityData target)
        {
            ID id = Get<ID>();
            if (id == null) return false;
            if (id.Faction == null) return false;
            ID targetId = target.Get<ID>();
            if (targetId == null) return false;
            if (targetId.Faction == null) return false;
            return targetId.Faction.IsHostile(id.Faction);
        }
    }
}