using Sentience;
using UnityEngine;

namespace MindTheatre
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "I_Close", menuName = "Scaerth/Interaction/Close")]
    public class I_Close : EntityInteraction
    {
        protected override bool IsPossible(Body self, EntityData interactor, EntityData target)
        {
            if (self.Entity is not Door door) return false;
            if (door.Open == false) return false;
            return true;
        }

        protected override bool TryExecute(Body self, EntityData interactor, EntityData target)
        {
            Door door = (Door) self.Entity;
            door.Open = false;
            return true;
        }
    }
}