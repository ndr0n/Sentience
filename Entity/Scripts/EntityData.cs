using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using MindTheatre;
using Unity.Rendering;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Sentience
{
    [System.Serializable]
    public class EntityData
    {
        public string Name;
        public EntityType Type;

        public List<EntityComponentData> Components;

        // readonly Dictionary<int, EntityComponent> componentList = new();
        bool spawned = false;

        public ID ID => Get<ID>();

        public EntityData(string name, string description, EntityType type, System.Random random)
        {
            Name = name;
            Type = type;
            spawned = false;
            Components = new();
            // componentList = new();

            ID id = new()
            {
                Description = description,
            };
            EntityComponentData idData = new(id);
            Components.Add(idData);

            foreach (var componentType in type.Components)
            {
                AddComponent(componentType.Authoring, random);
            }

            Init(random);
        }

        public void Init(System.Random random)
        {
            // SetupDictionary();
            if (spawned) return;
            spawned = true;
            if (random == null) random = new(Random.Range(0, int.MaxValue));
            foreach (var component in Components) component.Component.OnInit(this, random);
        }

        // public void SetupDictionary()
        // {
        // if (dictionary == null)
        // {
        //     dictionary = new();
        // }
        // if (componentList.Count == 0)
        // {
        // foreach (var component in Components)
        // {
        // componentList.Add(component.Component.GetType().GetHashCode(), component.Component);
        // }
        // }
        // }

        public bool Has<T>()
        {
            foreach (var component in Components)
            {
                if (component.Component.GetType() == typeof(T)) return true;
            }

            return false;
            // if (componentList == null) Init(null);
            // return componentList.ContainsKey(typeof(T).GetHashCode());
        }

        public T Get<T>() where T : EntityComponent
        {
            foreach (var component in Components)
            {
                if (component.Component.GetType() == typeof(T)) return component.Component as T;
            }

            return null;
            // if (componentList == null) Init(null);
            // return componentList[typeof(T).GetHashCode()] as T;
        }

        void AddComponent(EntityAuthoring authoring, System.Random random)
        {
            IEntityComponent component = authoring.Spawn(random);
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

        // public void SpawnDotsEntity(System.Random random)
        // {
        //     EntityManager em = World.DefaultGameObjectInjectionWorld.EntityManager;
        //     if (em.Exists(Entity)) return;
        //
        //     List<ComponentType> componentTypes = new();
        //     foreach (var c in Components) componentTypes.Add(c.Component.GetType());
        //     componentTypes.Add(typeof(Parent));
        //     componentTypes.Add(typeof(LocalToWorld));
        //     componentTypes.Add(typeof(LocalTransform));
        //     EntityArchetype archetype = em.CreateArchetype(componentTypes.ToArray());
        //
        //     Entity = em.CreateEntity(archetype);
        //
        //     foreach (var component in Components)
        //     {
        //         em.AddComponentObject(Entity, component.Component);
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