using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "IA_Give", menuName = "Sentience/Item/Interaction/Give")]
    public class IA_Give : ItemInteraction
    {
        protected override bool IsPossible(Item self, EntityData interactor, EntityData target)
        {
            if (interactor == target) return false;
            Inventory trg = target.Get<Inventory>();
            if (trg == null) return false;
            Inventory inv = interactor.Get<Inventory>();
            if (inv == null) return false;
            if (!inv.Items.Exists(x => x.Data == self.Data)) return false;
            return true;
        }

        protected override bool TryExecute(Item self, EntityData interactor, EntityData target)
        {
            Inventory trg = target.Get<Inventory>();
            trg.Add(self);
            Inventory inv = interactor.Get<Inventory>();
            inv.Remove(self, 1);
            return true;
        }
    }
}