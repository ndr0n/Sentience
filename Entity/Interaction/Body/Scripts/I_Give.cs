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

            Item item = self.Get<Item>();
            if (item == null) return false;

            Inventory giver = interactor.Get<Inventory>();
            if (giver == null) return false;
            if (!giver.Items.Exists(x => x.Item == item)) return false;

            Inventory receiver = target.Get<Inventory>();
            if (receiver == null) return false;

            return true;
        }

        protected override bool OnTryInteract(EntityData self, EntityData interactor, EntityData target)
        {
            Item item = self.Get<Item>();

            Inventory receiver = target.Get<Inventory>();
            receiver.Add(item);

            Inventory giver = interactor.Get<Inventory>();
            giver.Remove(item, 1);

            return true;
        }
    }
}