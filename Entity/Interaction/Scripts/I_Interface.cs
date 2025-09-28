using Sentience;
using UnityEngine;

namespace MindTheatre
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "I_Interface", menuName = "Sentience/Interaction/Interface")]
    public class I_Interface : EntityInteraction
    {
        protected override bool IsPossible(Body self, EntityData interactor, EntityData target)
        {
            return true;
        }

        protected override bool TryExecute(Body self, EntityData interactor, EntityData target)
        {
            return false;
        }
    }
}