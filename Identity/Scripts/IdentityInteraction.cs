using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    [System.Serializable]
    public class IdentityInteractionData
    {
        [SerializeReference] public IdentityData Target;
        [SerializeReference] public IdentityInteraction Interaction;

        public IdentityInteractionData(IdentityData target, IdentityInteraction interaction)
        {
            Target = target;
            Interaction = interaction;
        }
    }

    public abstract class IdentityInteraction : ScriptableObject
    {
        public string Name = "Interaction";
        public string Description = "";
        public abstract bool IsPossible(Identity self);
    }
}