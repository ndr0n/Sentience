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

        protected override bool OnTryInteract(EntityData self, EntityData interactor, EntityData target)
        {
            Item item = self.Get<Item>();
            Inventory giver = interactor.Get<Inventory>();
            Inventory receiver = target.Get<Inventory>();

            receiver.Add(item, 1);
            giver.Remove(item, 1);

            return true;
        }
    }
}