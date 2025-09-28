using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    public class ComponentData
    {
        [HideInInspector] public string Name;
        [SerializeReference] public Component Component;
        [HideInInspector] public int Hash;

        public ComponentData(Component component)
        {
            string type = component.GetType().ToString();
            Name = type.Split('.')[^1];
            Hash = type.GetHashCode();
            Component = component;
        }
    }

    [System.Serializable]
    public class EntityData
    {
        public string Name;
        public string Description;
        public EntityType Type;
        [SerializeReference] public List<ComponentData> Components = new();

        public EntityData(string name, string description, EntityType type, List<Component> components, System.Random random)
        {
            Name = name;
            Description = description;
            Type = type;
            Components = new();
            foreach (var component in components) Components.Add(new(component));
            type.SpawnData(this, random);
        }

        public T Get<T>() where T : Component
        {
            // int hash = typeof(T).GetHashCode();
            foreach (var c in Components)
            {
                if (c.Component is T t) return t;
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