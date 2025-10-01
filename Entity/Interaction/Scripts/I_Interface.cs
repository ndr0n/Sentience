using Sentience;
using Unity.Entities;
using UnityEngine;

namespace MindTheatre
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "I_Interface", menuName = "Sentience/Interaction/Interface")]
    public class I_Interface : Interaction
    {
        public override bool HasInteraction(EntityData self, EntityData interactor, EntityData target)
        {
            return true;
        }

        public override bool Interact(ref SystemState state, RefRW<InteractionComponent> comp)
        {
            return true;
        }
    }
}