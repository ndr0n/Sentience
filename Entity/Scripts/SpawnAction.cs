using System;
using UnityEngine;

namespace Sentience
{
    public abstract class SpawnAction : ScriptableObject
    {
        public string Name = "Action";
        public string Description = "";
        public string Tags = "";

        public abstract bool CanExecute(Spawn self, Vector3 position, Spawn target);
        public abstract bool TryExecute(Spawn self, Vector3 position, Spawn target);
    }
}