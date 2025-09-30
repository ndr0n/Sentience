using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    [System.Serializable]
    public class EntityData
    {
        [HideInInspector] public string Name;
        public List<EntityComponentData> Components;

        public EntityData(string name, string description, EntityType type, System.Random random)
        {
            Name = name;
            Components = new();

            Info d = new();
            d.Name = name;
            d.Description = description;
            d.Type = type;
            Components.Add(new(d));

            foreach (var componentType in type.Components)
            {
                AddComponent(componentType.Authoring, random);
            }
        }

        public bool Has<T>()
        {
            return Components.Exists(x => x.Component is T);
        }

        public T Get<T>() where T : class
        {
            return Components.FirstOrDefault(x => x.Component is T)?.Component as T;
        }

        void AddComponent(EntityAuthoring authoring, System.Random random)
        {
            IEntityComponent component = authoring.Spawn(random);
            component.Init(this, random);
            EntityComponentData componentData = new(component);
            Components.Add(componentData);
        }

        public bool IsHostile(EntityData target)
        {
            if (!Has<Identity>()) return false;
            Identity identity = Get<Identity>();
            if (identity.Faction == null) return false;

            if (!target.Has<Identity>()) return false;
            Identity targetIdentity = target.Get<Identity>();
            if (targetIdentity.Faction == null) return false;

            return targetIdentity.Faction.IsHostile(identity.Faction);
        }

        // public Entity Entity;
        //
        // public void SpawnDotsEntity(System.Random random)
        // {
        //     EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        //
        //     List<ComponentType> componentTypes = new();
        //     foreach (var c in Components) componentTypes.Add(c.Component.GetType());
        //     EntityArchetype archetype = entityManager.CreateArchetype(componentTypes.ToArray());
        //
        //     Entity = entityManager.CreateEntity(archetype);
        //
        //     foreach (var component in Components)
        //     {
        //         entityManager.AddComponentObject(Entity, component.Component);
        //     }
        // }
    }

    [System.Serializable]
    public class EntityComponentData
    {
        [HideInInspector] public string Name;
        [SerializeReference] public IEntityComponent Component;

        public EntityComponentData(IEntityComponent component)
        {
            string type = component.GetType().ToString();
            Name = type.Split('.')[^1];
            Component = component;
        }
    }
}