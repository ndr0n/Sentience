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

        protected override bool OnTryInteract(EntityData self, EntityData interactor, EntityData target)
        {
            Item item = self.Get<Item>();
            Inventory retriever = interactor.Get<Inventory>();
            Inventory owner = target.Get<Inventory>();

            retriever.Add(item);
            owner.Remove(item, 1);

            return true;
        }
    }
}