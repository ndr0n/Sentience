using Sentience;
using UnityEngine;

namespace MindTheatre
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "I_Open", menuName = "Scaerth/Interaction/Open")]
    public class I_Open : Interaction
    {
        public override bool HasInteraction(EntityData self, EntityData interactor, EntityData target)
        {
            Lock _lock = self.Get<Lock>();
            if (_lock == null) return false;
            if (_lock.Open) return false;
            return true;
        }

        protected override bool OnTryInteract(EntityData self, EntityData interactor, EntityData target)
        {
            Lock _lock = self.Get<Lock>();
            _lock.Open = false;
            return true;
        }
    }
}