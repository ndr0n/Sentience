using System;
using UnityEngine;

namespace Sentience
{
    public abstract class Action : ScriptableObject
    {
        public string Name = "Action";
        public string Description = "";
        public string Tags = "";

        public abstract bool CanExecute(Entity self, Vector3 position, Entity target);
        public abstract bool TryExecute(Entity self, Vector3 position, Entity target);
    }
}