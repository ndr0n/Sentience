using Sentience;
using Unity.Entities;
using UnityEngine;

namespace MindTheatre
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "I_Open", menuName = "Scaerth/Interaction/Open")]
    public class I_Open : Interaction
    {
        public override bool HasInteraction(EntityData self, EntityData interactor, EntityData target)
        {
            if (!self.Has<Lock>()) return false;
            Lock _lock = self.Get<Lock>();
            if (_lock.Open) return false;
            return true;
        }

        public override bool Interact(ref SystemState state, RefRW<InteractionComponent> comp)
        {
            Lock _lock = state.EntityManager.GetComponentObject<Lock>(comp.ValueRO.Self);
            _lock.Open = true;
            return true;
        }
    }
}