using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    public struct UpdatePositionComponent : IComponentData
    {
        public Vector3 WorldPosition;
    }
}