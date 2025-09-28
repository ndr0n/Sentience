using Sentience;
using UnityEngine;

namespace MindTheatre
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "I_Open", menuName = "Scaerth/Interaction/Open")]
    public class I_Open : EntityInteraction
    {
        protected override bool IsPossible(Body self, EntityData interactor, EntityData target)
        {
            if (self.Entity is not Door door) return false;
            if (door.Open) return false;
            return true;
        }

        protected override bool TryExecute(Body self, EntityData interactor, EntityData target)
        {
            Door door = (Door) self.Entity;
            door.Open = true;
            return true;
        }
    }
}