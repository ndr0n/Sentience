using Sentience;
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

        protected override bool OnTryInteract(EntityData self, EntityData interactor, EntityData target)
        {
            Hud.Instance.InventoryPanel.Show(true, self);
            return true;
        }
    }
}