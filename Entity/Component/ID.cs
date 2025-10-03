using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

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
    }
}