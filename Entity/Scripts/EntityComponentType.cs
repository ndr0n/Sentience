using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    [System.Serializable]
    public class EntityComponentType
    {
        [HideInInspector] public string Name;
        [SerializeReference] public EntityAuthoring Authoring;

        public EntityComponentType(string name, EntityAuthoring authoring)
        {
            Name = name;
            Authoring = authoring;
        }
    }
}