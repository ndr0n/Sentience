using System;
using UnityEngine;

namespace Sentience
{
    public abstract class SpawnAction : ScriptableObject
    {
        public string Name = "Action";
        public string Description = "";
        public string Tags = "";

        public abstract bool CanExecute(ID self, Vector3 position, ID target);
        public abstract bool TryExecute(ID self, Vector3 position, ID target);
    }
}