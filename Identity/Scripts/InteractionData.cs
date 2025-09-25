using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    public class InteractionData
    {
        [SerializeReference] public Item Item;
        public ItemInteraction ItemInteraction;
        [SerializeReference] public IdentityData Target;
        public IdentityInteraction TargetInteraction;

        public InteractionData(Item item, ItemInteraction itemInteraction, IdentityData target, IdentityInteraction targetInteraction)
        {
            Item = item;
            ItemInteraction = itemInteraction;
            Target = target;
            TargetInteraction = targetInteraction;
        }
    }
}