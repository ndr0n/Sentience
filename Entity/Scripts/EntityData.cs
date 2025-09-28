using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    [System.Serializable]
    public class EntityComponentData
    {
        [HideInInspector] public string Name;
        [SerializeReference] public IEntityComponent Component;
        [HideInInspector] public int Hash;

        public EntityComponentData(IEntityComponent component)
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
        [SerializeReference] List<EntityComponentData> Components = new();

        public EntityData(string name, string description, EntityType type, System.Random random)
        {
            Name = name;
            Description = description;
            Type = type;
            Components = new();

            foreach (var componentType in type.Components)
            {
                IEntityComponent component = componentType.Authoring.Spawn(random);
                EntityComponentData componentData = new(component);
                Components.Add(componentData);
            }

            type.SpawnData(this, random);

            foreach (var component in Components)
            {
                component.Component.Init(this, random);
            }

            // InitEcs(random);
        }

        // void InitEcs(System.Random random)
        // {
        //     entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        //     // List<Unity.Entities.ComponentType> types = new();
        //     // foreach (var c in Components) types.Add(c.EntityComponent.GetType());
        //     // EntityArchetype archetype = entityManager.CreateArchetype(types.ToArray());
        //
        //     // Attributes = entityManager.GetComponentData<Attributes>(Entity);
        //
        //     Entity = entityManager.CreateEntity();
        //
        //     List<Type> types = new();
        //     foreach (var component in Components)
        //     {
        //         Type type = component.Component.GetType();
        //         entityManager.AddComponent(Entity, type);
        //         types.Add(type);
        //     }
        //
        //     for (int i = 0; i < Components.Count; i++)
        //     {
        //         Type genericType = typeof(EntityManager).MakeGenericType(new Type[] {Components[i].GetType()});
        //         object res = genericType.GetMethod("GetComponentData").Invoke(entityManager, new object[] {Entity});
        //         Components[i].Component = res as EntityComponent;
        //     }
        // }

        public T Get<T>() where T : IEntityComponent
        {
            // int hash = typeof(T).GetHashCode();
            foreach (var c in Components)
            {
                if (c.Component is T t) return t;
            }
            return default;
            // return entityManager.GetComponentObject<T>(Entity);
        }

        public virtual bool IsHostile(EntityData target)
        {
            Identity identity = Get<Identity>();
            if (identity == null) return false;
            if (identity.Faction == null) return false;
            Identity targetIdentity = target.Get<Identity>();
            if (targetIdentity == null) return false;
            if (targetIdentity.Faction == null) return false;
            return targetIdentity.Faction.IsHostile(identity.Faction);
        }
    }
}