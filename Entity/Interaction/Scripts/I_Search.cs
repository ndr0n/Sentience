using Sentience;
using Unity.Entities;
using UnityEngine;

namespace MindTheatre
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "I_Search", menuName = "Scaerth/Interaction/Search")]
    public class I_Search : Interaction
    {
        public override bool HasInteraction(EntityData self, EntityData interactor, EntityData target)
        {
            if (!self.Has<Inventory>()) return false;
            return true;
        }

        public override bool Interact(ref SystemState state, RefRW<InteractionComponent> comp)
        {
            ID id = state.EntityManager.GetComponentObject<ID>(comp.ValueRO.Interactor);
            Hud.Instance.InventoryPanel.Show(true, id.Data);
            return true;
        }
    }
}