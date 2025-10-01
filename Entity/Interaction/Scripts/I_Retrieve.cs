using Unity.Entities;
using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "I_Retrieve", menuName = "Sentience/Interaction/Retrieve")]
    public class I_Retrieve : Interaction
    {
        public override bool HasInteraction(EntityData self, EntityData interactor, EntityData target)
        {
            if (interactor == target) return false;

            if (!self.Has<Item>()) return false;

            if (!target.Has<Inventory>()) return false;
            Inventory owner = target.Get<Inventory>();
            if (!owner.Items.Exists(x => x.Item.Data == self)) return false;

            if (!interactor.Has<Inventory>()) return false;

            return true;
        }

        public override bool Interact(ref SystemState state, RefRW<InteractionComponent> comp)
        {
            Item item = state.EntityManager.GetComponentObject<Item>(comp.ValueRO.Self);
            Inventory retriever = state.EntityManager.GetComponentObject<Inventory>(comp.ValueRO.Interactor);
            Inventory owner = state.EntityManager.GetComponentObject<Inventory>(comp.ValueRO.Target);

            retriever.Add(item);
            owner.Remove(item, 1);

            return true;
        }
    }
}