using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Unity.Entities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    [System.Serializable]
    public class EntityData
    {
        [HideInInspector] public string Name;
        public List<EntityComponentData> Components;
        // [SerializeField] Dictionary<int, IEntityComponent> dictionary;

        public EntityData(string name, string description, EntityType type, System.Random random)
        {
            Name = name;
            Components = new();

            ID id = new()
            {
                Name = name,
                Description = description,
                Type = type,
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
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            if (!entityManager.Exists(Entity))
            {
                SpawnDotsEntity(random);
            }

            foreach (var component in Components)
            {
                if (component.Component is IEntityComponent c) c.Init(this, random);
            }

            if (entityManager.HasComponent<Inventory>(Entity))
            {
                Inventory inv = entityManager.GetComponentObject<Inventory>(Entity);
                foreach (var item in inv.Items) item.Item.Init(random);
            }

            if (entityManager.HasComponent<Equipment>(Entity))
            {
                Equipment equipment = entityManager.GetComponentData<Equipment>(Entity);
                equipment.MeleeWeapon.Init(random);
                equipment.RangedWeapon.Init(random);
            }
        }

        public bool Has<T>()
        {
            return World.DefaultGameObjectInjectionWorld.EntityManager.HasComponent<T>(Entity);
            // return Components.Exists(x => x.Component is T);
            // return dictionary.ContainsKey(typeof(T).GetHashCode());
        }

        public T Get<T>() where T : EntityComponent
        {
            return World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentObject<T>(Entity);
            // return Components.FirstOrDefault(x => x.Component is T).Component as T;
            // return dictionary[typeof(T).GetHashCode()] as T;
        }

        public T GetData<T>() where T : unmanaged, IComponentData
        {
            return World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<T>(Entity);
            // return Components.FirstOrDefault(x => x.Component is T).Component as T;
            // return dictionary[typeof(T).GetHashCode()] as T;
        }

        public void SetData<T>(T data) where T : unmanaged, IComponentData
        {
            World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData(Entity, data);
            // return Components.FirstOrDefault(x => x.Component is T).Component as T;
            // return dictionary[typeof(T).GetHashCode()] as T;
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

        public Entity Entity;

        public void SpawnDotsEntity(System.Random random)
        {
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            if (entityManager.Exists(Entity)) return;

            List<ComponentType> componentTypes = new();
            foreach (var c in Components) componentTypes.Add(c.Component.GetType());
            EntityArchetype archetype = entityManager.CreateArchetype(componentTypes.ToArray());

            Entity = entityManager.CreateEntity(archetype);

            foreach (var component in Components)
            {
                if (component.Component is ID id)
                {
                    id.Entity = Entity;
                    World.DefaultGameObjectInjectionWorld.EntityManager.AddComponentData(Entity, id);
                }
                else
                {
                    entityManager.AddComponentObject(Entity, component.Component);
                }
            }
        }
    }

    [System.Serializable]
    public class EntityComponentData
    {
        [HideInInspector] public string Name;
        [SerializeReference] public IComponentData Component;

        public EntityComponentData(IComponentData component)
        {
            string type = component.GetType().ToString();
            Name = type.Split('.')[^1];
            Component = component;
        }
    }
}