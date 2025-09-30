using Sentience;
using Unity.Entities;
using UnityEngine;

namespace MindTheatre
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "I_Open", menuName = "Scaerth/Interaction/Open")]
    public class I_Open : Interaction
    {
        public override bool HasInteraction(Entity self, Entity interactor, Entity target)
        {
            if (!EntityLibrary.Has<Lock>(self)) return false;
            Lock _lock = EntityLibrary.Get<Lock>(self);
            if (_lock.Open) return false;

            return true;
        }

        protected override bool OnTryInteract(Entity self, Entity interactor, Entity target)
        {
            Lock _lock = EntityLibrary.Get<Lock>(self);
            _lock.Open = true;

            return true;
        }
    }
}