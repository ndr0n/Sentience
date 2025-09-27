using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "IA_Give", menuName = "Sentience/Item/Interaction/Give")]
    public class IA_Give : ItemInteraction
    {
        protected override bool OnHasInteraction(Item self, EntityData interactor, EntityData target)
        {
            if (interactor == target) return false;

            if (target is not IdentityData targetData) return false;
            if (targetData.Inventory == null) return false;

            if (interactor is not IdentityData interactorData) return false;
            if (interactorData.Inventory == null) return false;
            if (!interactorData.Inventory.Items.Contains(self)) return false;

            return true;
        }

        protected override bool TryExecute(Item self, EntityData interactor, EntityData target)
        {
            IdentityData targetDAta = target as IdentityData;
            targetDAta.Inventory.Add(self);

            IdentityData giver = interactor as IdentityData;
            giver.Inventory.Remove(self, 1);

            return true;
        }
    }
}