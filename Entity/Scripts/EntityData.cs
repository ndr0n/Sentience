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

namespace Sentience
{
    [System.Serializable]
    public class EntityData
    {
        [HideInInspector] public string Name;
        public List<EntityComponentData> Components;
        readonly Dictionary<int, IEntityComponent> dictionary = new();

        public EntityData(string name, string description, EntityType type, System.Random random)
        {
            Name = name;
            Components = new();
            dictionary = new();

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
            if (dictionary.Count == 0)
            {
                foreach (var component in Components)
                {
                    dictionary.Add(component.Component.GetType().GetHashCode(), component.Component);
                    component.Component.Init(this, random);
                }

                if (Has<Inventory>())
                {
                    Inventory inv = Get<Inventory>();
                    foreach (var item in inv.Items) item.Item.Init(random);
                }

                if (Has<Equipment>())
                {
                    Equipment equip = Get<Equipment>();
                    equip.MeleeWeapon.Init(random);
                    equip.RangedWeapon.Init(random);
                }
            }

            // EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            // if (!entityManager.Exists(Entity))
            // {
            //     SpawnDotsEntity(random);
            //
            //     foreach (var component in Components)
            //     {
            //         if (component.Component is IEntityComponent c) c.Init(this, random);
            //     }
            //
            //     if (entityManager.HasComponent<Inventory>(Entity))
            //     {
            //         Inventory inv = entityManager.GetComponentObject<Inventory>(Entity);
            //         foreach (var item in inv.Items)
            //         {
            //             item.Item.Init(random);
            //             entityManager.AddComponentData<SetParentComponent>(item.Item.Entity, new() {Parent = Entity});
            //         }
            //     }
            //
            //     if (entityManager.HasComponent<Equipment>(Entity))
            //     {
            //         Equipment equipment = entityManager.GetComponentData<Equipment>(Entity);
            //         equipment.MeleeWeapon.Init(random);
            //         equipment.RangedWeapon.Init(random);
            //         entityManager.AddComponentData<SetParentComponent>(equipment.MeleeWeapon.Entity, new() {Parent = Entity});
            //         entityManager.AddComponentData<SetParentComponent>(equipment.RangedWeapon.Entity, new() {Parent = Entity});
            //     }
            // }
        }

        public bool Has<T>()
        {
            return dictionary.ContainsKey(typeof(T).GetHashCode());
            // return World.DefaultGameObjectInjectionWorld.EntityManager.HasComponent<T>(Entity);
        }

        public T Get<T>() where T : EntityComponent
        {
            return dictionary[typeof(T).GetHashCode()] as T;
            // return World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentObject<T>(Entity);
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