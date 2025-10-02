using Unity.Collections;
using Unity.Entities;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Sentience
{
    [System.Serializable]
    public struct ID : IComponentData
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

        public FixedString64Bytes type;
        public EntityType Type
        {
            get { return (EntityType) UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings.FindAssetEntry(type.Value).MainAsset; }
            set { type = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(value)); }
        }

        public Vector3 position;
        public Vector3 Position
        {
            get { return position; }
            set
            {
                position = value;
                World.DefaultGameObjectInjectionWorld.EntityManager.AddComponent<PositionChangedComponent>(Entity);
            }
        }

        Entity entity;
        public Entity Entity
        {
            get { return entity; }
            set { entity = value; }
        }

        // public ID(string name, string description, EntityType type, Vector3 position)
        // {
        //     this.name = name;
        //     this.description = description;
        //     this.type = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(type));
        //     this.position = position;
        // }
    }
}