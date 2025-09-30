using Sentience;
using Unity.Entities;
using UnityEngine;

namespace MindTheatre
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "I_Search", menuName = "Scaerth/Interaction/Search")]
    public class I_Search : Interaction
    {
        public override bool HasInteraction(Entity self, Entity interactor, Entity target)
        {
            if (!EntityLibrary.Has<Inventory>(self)) return false;
            return true;
        }

        protected override bool OnTryInteract(Entity self, Entity interactor, Entity target)
        {
            Hud.Instance.InventoryPanel.Show(true, self);
            return true;
        }
    }
}