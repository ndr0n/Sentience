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

        protected override bool OnTryInteract(EntityData self, EntityData interactor, EntityData target)
        {
            return true;
        }
    }
}