using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "IA_Retrieve", menuName = "Sentience/Item/Interaction/Retrieve")]
    public class IA_Retrieve : ItemInteraction
    {
        protected override bool IsPossible(Item self, EntityData interactor, EntityData target)
        {
            if (interactor == target) return false;
            Inventory inv = interactor.Get<Inventory>();
            if (inv == null) return false;
            return true;
        }

        protected override bool TryExecute(Item self, EntityData interactor, EntityData target)
        {
            Inventory inv = interactor.Get<Inventory>();
            inv.Add(self);

            Inventory own = target.Get<Inventory>();
            if (own != null) own.Remove(self, 1);

            return true;
        }
    }
}