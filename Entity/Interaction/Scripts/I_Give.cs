using Unity.Entities;
using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "I_Give", menuName = "Sentience/Interaction/Give")]
    public class I_Give : Interaction
    {
        public override bool HasInteraction(EntityData self, EntityData interactor, EntityData target)
        {
            if (interactor == target) return false;

            if (!self.Has<Item>()) return false;

            if (!interactor.Has<Inventory>()) return false;
            Inventory giver = interactor.Get<Inventory>();
            if (!giver.Items.Exists(x => x.Item.Data == self)) return false;

            return true;
        }

        public override bool Interact(ref SystemState state, RefRW<InteractionComponent> comp)
        {
            Item item = state.EntityManager.GetComponentObject<Item>(comp.ValueRO.Self);
            Inventory giver = state.EntityManager.GetComponentObject<Inventory>(comp.ValueRO.Interactor);
            Inventory receiver = state.EntityManager.GetComponentObject<Inventory>(comp.ValueRO.Target);

            receiver.Add(item);
            giver.Remove(item, 1);

            return true;
        }
    }
}