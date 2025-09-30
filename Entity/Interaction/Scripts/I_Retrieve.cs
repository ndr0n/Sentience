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

            Item item = self.Get<Item>();
            if (item == null) return false;

            Inventory owner = target.Get<Inventory>();
            if (owner == null) return false;
            if (!owner.Items.Exists(x => x.Item == item)) return false;

            Inventory retriever = interactor.Get<Inventory>();
            if (retriever == null) return false;

            return true;
        }

        protected override bool OnTryInteract(EntityData self, EntityData interactor, EntityData target)
        {
            Item item = self.Get<Item>();

            Inventory retriever = interactor.Get<Inventory>();
            retriever.Add(item);
            
            Inventory owner = target.Get<Inventory>();
            owner.Remove(item, 1);

            return true;
        }
    }
}