using Sentience;
using UnityEngine;

namespace MindTheatre
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "I_Close", menuName = "Sentience/Interaction/Close")]
    public class I_Close : Interaction
    {
        public override bool HasInteraction(EntityData self, EntityData interactor, EntityData target)
        {
            Lock _lock = self.Get<Lock>();
            if (_lock == null) return false;
            if (_lock.Open == false) return false;
            return true;
        }

        protected override bool OnTryInteract(EntityData self, EntityData interactor, EntityData target)
        {
            Lock _lock = self.Get<Lock>();
            _lock.Open = true;
            return true;
        }
    }
}