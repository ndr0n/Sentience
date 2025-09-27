using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    [System.Serializable]
    public class InteractionData
    {
        [SerializeReference] public Item Item;
        [SerializeReference] public IdentityData Target;
        public Interaction TargetInteraction;

        public InteractionData(Item item, IdentityData target, Interaction targetInteraction)
        {
            Item = item;
            Target = target;
            TargetInteraction = targetInteraction;
        }
    }
}