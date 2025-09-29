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
        // Dictionary<int, IEntityComponent> dictionary = new();

        public EntityData(string name, string description, EntityType type, System.Random random)
        {
            Name = name;
            Description = description;
            Type = type;
            // dictionary.Clear();
            Components = new();

            foreach (var componentType in type.Components) AddComponent(componentType.Authoring, random);
            type.SpawnData(this, random);
            foreach (var component in Components) component.Component.Init(this, random);

            // InitEcs(random);
        }

        void AddComponent(EntityComponentAuthoring authoring, System.Random random)
        {
            IEntityComponent component = authoring.Spawn(random);
            EntityComponentData componentData = new(component);
            Components.Add(componentData);
            // dictionary.Add(componentData.Component.GetType().GetHashCode(), componentData.Component);
        }

        void RemoveComponent(IEntityComponent component)
        {
            EntityComponentData data = Components.First(x => x.Component == component);
            Components.Remove(data);
            // dictionary.Remove(component.GetType().GetHashCode());
        }

        public T Get<T>() where T : class, IEntityComponent
        {
            return Components.FirstOrDefault(x => x.Component is T)?.Component as T;
            // int hash = typeof(T).GetHashCode();
            // return dictionary[hash] as T;
            // foreach (var c in Components)
            // {
            // if (c.Component is T t) return t;
            // }
            // return null;
            // return World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentObject<T>(Entity);
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

        #region UnityECS

        // void InitEcs(System.Random random)
        // {
        //     EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        //
        //     List<Unity.Entities.ComponentType> componentTypes = new();
        //     foreach (var c in Components) componentTypes.Add(c.Component.GetType());
        //     EntityArchetype archetype = entityManager.CreateArchetype(componentTypes.ToArray());
        //     Entity = entityManager.CreateEntity(archetype);
        //
        //     for (int i = 0; i < Components.Count; i++)
        //     {
        //         Type type = Components[i].Component.GetType();
        //         entityManager.AddComponent(Entity, type);
        //         // var attributes = entityManager.GetComponentData<Attributes>(Entity);
        //         // entityManager.SetComponentData(Entity, );
        //         // var data = Components[i] as IEntityComponentData
        //         // entityManager.SetComponentData(Entity, Components[i].Component);
        //
        //         if (Components[i].Component is Body body)
        //         {
        //             entityManager.SetComponentData<Body>(Entity, body);
        //             Debug.Log($"ENTITY -> SET BODY DATA: {body.Data.Name}");
        //             Components[i].Component = entityManager.GetComponentData<Body>(Entity);
        //             Debug.Log($"ENTITY -> GET BODY DATA: {body.Data.Name}");
        //         }
        //     }
        //
        //     // for (int i = 0; i < Components.Count; i++)
        //     // {
        //     // Type genericType = typeof(EntityManager).MakeGenericType(new Type[] {Components[i].GetType()});
        //     // object res = genericType.GetMethod("GetComponentData").Invoke(entityManager, new object[] {Entity});
        //     // Components[i].Component = res as EntityComponent;
        //     // }
        // }

        #endregion
    }
}