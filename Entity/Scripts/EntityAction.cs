using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sentience
{
    public abstract class EntityAction : ScriptableObject
    {
        public string Name = "Action";
        public string Description = "";
        public string Tags = "";

        public abstract bool CanExecute(ID self, Vector3 position, ID target);
        public abstract Coroutine TryExecute(ID self, Vector3 position, ID target);
    }
}