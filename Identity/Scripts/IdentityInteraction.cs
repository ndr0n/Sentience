using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    public abstract class IdentityInteraction : ScriptableObject
    {
        public string Name = "Interaction";
        public string Description = "";
        public abstract bool IsPossible(Identity self);
    }
}