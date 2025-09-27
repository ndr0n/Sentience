using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "IA_Retrieve", menuName = "Sentience/Item/Interaction/Retrieve")]
    public class IA_Retrieve : ItemInteraction
    {
        protected override bool OnHasInteraction(Item self, EntityData interactor)
        {
            if (interactor is not IdentityData data) return false;
            if (data.Inventory == null) return false;
            return true;
        }

        protected override bool TryExecute(Item self, EntityData interactor, EntityData target)
        {
            IdentityData data = interactor as IdentityData;
            data.Inventory.Add(self);
            if (target is IdentityData owner) owner.Inventory.Remove(self, 1);
            return true;
        }
    }
}