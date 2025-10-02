using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Sentience
{
    public class ID : EntityComponent
    {
        public string Name;

        public string Description;

        public EntityType Type;

        [SerializeField] Vector3 position;
        public Vector3 Position
        {
            get => position;
            set
            {
                position = value;
                EntityManager em = World.DefaultGameObjectInjectionWorld.EntityManager;
                em.AddComponentData(Data.Entity, new UpdatePositionComponent() {WorldPosition = value});
                OnChangePosition?.Invoke(value);
                if (em.HasBuffer<Child>(Data.Entity))
                {
                    List<Entity> children = new();

                    DynamicBuffer<Child> childrenBuffer = em.GetBuffer<Child>(Data.Entity);
                    foreach (var childBuffer in childrenBuffer) children.Add(childBuffer.Value);

                    foreach (var child in children)
                    {
                        ID id = em.GetComponentObject<ID>(child);
                        id.Position = value;
                    }
                }
            }
        }

        public Action<Vector3> OnChangePosition;
    }

    [System.Serializable]
    public struct IDComponent : IComponentData
    {
        FixedString128Bytes name;
        public string Name
        {
            get { return name.Value; }
            set { name = value; }
        }

        FixedString512Bytes description;
        public string Description
        {
            get { return description.Value; }
            set { description = value; }
        }

        FixedString64Bytes type;
        public EntityType Type
        {
            get { return (EntityType) UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings.FindAssetEntry(type.Value).MainAsset; }
            set { type = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(value)); }
        }

        Vector3 position;
        public Vector3 Position
        {
            get { return position; }
            set
            {
                position = value;
                World.DefaultGameObjectInjectionWorld.EntityManager.AddComponentData(Entity, new UpdatePositionComponent() {WorldPosition = value});
            }
        }

        public Entity Entity;
    }
}