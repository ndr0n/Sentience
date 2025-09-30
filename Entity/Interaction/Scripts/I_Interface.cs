using Sentience;
using Unity.Entities;
using UnityEngine;

namespace MindTheatre
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "I_Interface", menuName = "Sentience/Interaction/Interface")]
    public class I_Interface : Interaction
    {
        public override bool HasInteraction(Entity self, Entity interactor, Entity target)
        {
            return true;
        }

        protected override bool OnTryInteract(Entity self, Entity interactor, Entity target)
        {
            return true;
        }
    }
}