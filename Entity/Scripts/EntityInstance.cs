using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Unity.Entities;

namespace Sentience
{
    [System.Serializable]
    public class EntityInstance
    {
        [HideInInspector] public string Name;
        public Entity Entity;
        public List<EntityComponentInstance> Components;

        public EntityInstance(string name, string description, EntityType type, System.Random random)
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

            InitEcs(random);
        }

        void AddComponent(EntityAuthoring authoring, System.Random random)
        {
            IComponentData component = authoring.Spawn(random);
            EntityComponentInstance componentInstance = new(component);
            Components.Add(componentInstance);
        }

        #region UnityECS

        void InitEcs(System.Random random)
        {
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            List<ComponentType> componentTypes = new();
            foreach (var c in Components) componentTypes.Add(c.Component.GetType());
            EntityArchetype archetype = entityManager.CreateArchetype(componentTypes.ToArray());

            Entity = entityManager.CreateEntity(archetype);

            foreach (var component in Components)
            {
                if (component.Component is IEntityComponent ec)
                {
                    ec.Init(Entity, random);
                    entityManager.AddComponentObject(Entity, ec);
                }
            }

            // for (int i = 0; i < Components.Count; i++)
            // {
            // entityManager.AddComponentObject(Entity, Components[i].Component);
            // Type type = Components[i].Component.GetType();
            // var attributes = entityManager.GetComponentData<Attributes>(Entity);
            // entityManager.SetComponentData(Entity, );
            // var data = Components[i] as IEntityComponentData
            // entityManager.SetComponentData(Entity, Components[i].Component);

            // if (Components[i].Component is Body body)
            // {
            // entityManager.SetComponentData<Body>(Entity, body);
            // Debug.Log($"ENTITY -> SET BODY DATA: {body.Data.Name}");
            // Components[i].Component = entityManager.GetComponentData<Body>(Entity);
            // Debug.Log($"ENTITY -> GET BODY DATA: {body.Data.Name}");
            // }
            // }

            // for (int i = 0; i < Components.Count; i++)
            // {
            // Type genericType = typeof(EntityManager).MakeGenericType(new Type[] {Components[i].GetType()});
            // object res = genericType.GetMethod("GetComponentData").Invoke(entityManager, new object[] {Entity});
            // Components[i].Component = res as EntityComponent;
            // }
        }

        #endregion
    }

    [System.Serializable]
    public struct EntityComponentInstance
    {
        [HideInInspector] public string Name;
        [SerializeReference] public IComponentData Component;

        public EntityComponentInstance(IComponentData component)
        {
            string type = component.GetType().ToString();
            Name = type.Split('.')[^1];
            Component = component;
        }
    }
}