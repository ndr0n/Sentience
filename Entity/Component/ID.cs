using System;
using System.Collections.Generic;
using Unity.Collections;
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
                OnUpdatePosition?.Invoke(position);
            }
        }

        [SerializeField] Vector3 rotation;
        public Vector3 Rotation
        {
            get => rotation;
            set
            {
                rotation = value;
                OnUpdateRotation?.Invoke(rotation);
            }
        }

        public Action<Vector3> OnUpdatePosition;
        public Action<Vector3> OnUpdateRotation;

        // void UpdatePosition(Vector3 pos, Vector3 rot)
        // {
        // OnUpdatePosition?.Invoke(pos);

        // EntityManager em = World.DefaultGameObjectInjectionWorld.EntityManager;
        // em.AddComponentData(Data.Entity, new UpdatePositionComponent() {WorldPosition = pos, WorldRotation = Quaternion.Euler(rot)});
        // if (em.HasBuffer<Child>(Data.Entity))
        // {
        // List<Entity> children = new();

        // DynamicBuffer<Child> childrenBuffer = em.GetBuffer<Child>(Data.Entity);
        // foreach (var childBuffer in childrenBuffer) children.Add(childBuffer.Value);

        // foreach (var child in children)
        // {
        // ID id = em.GetComponentObject<ID>(child);
        // id.Position = pos;
        // id.Rotation = rot;
        // }
        // }
        // }
    }

    // [System.Serializable]
    // public struct IDComponent : IComponentData
    // {
    //     FixedString128Bytes name;
    //     public string Name
    //     {
    //         get { return name.Value; }
    //         set { name = value; }
    //     }
    //
    //     FixedString512Bytes description;
    //     public string Description
    //     {
    //         get { return description.Value; }
    //         set { description = value; }
    //     }
    //
    //     FixedString64Bytes type;
    //     public EntityType Type
    //     {
    //         get { return (EntityType) UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings.FindAssetEntry(type.Value).MainAsset; }
    //         set { type = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(value)); }
    //     }
    //
    //     Vector3 position;
    //     public Vector3 Position
    //     {
    //         get { return position; }
    //         set
    //         {
    //             position = value;
    //             World.DefaultGameObjectInjectionWorld.EntityManager.AddComponentData(Entity, new UpdatePositionComponent() {WorldPosition = value});
    //         }
    //     }
    //
    //     public Entity Entity;
    // }
}