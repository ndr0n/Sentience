using Unity.Entities;
using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "I_Retrieve", menuName = "Sentience/Interaction/Retrieve")]
    public class I_Retrieve : Interaction
    {
        public override bool HasInteraction(Entity self, Entity interactor, Entity target)
        {
            if (interactor == target) return false;

            if (!EntityLibrary.Has<Item>(self)) return false;

            if (!EntityLibrary.Has<Inventory>(target)) return false;
            Inventory owner = EntityLibrary.Get<Inventory>(target);
            if (!owner.Items.Exists(x => x.Item.Entity == self)) return false;

            if (!EntityLibrary.Has<Inventory>(interactor)) return false;

            return true;
        }

        protected override bool OnTryInteract(Entity self, Entity interactor, Entity target)
        {
            Item item = EntityLibrary.Get<Item>(self);
            Inventory retriever = EntityLibrary.Get<Inventory>(interactor);
            Inventory owner = EntityLibrary.Get<Inventory>(target);

            retriever.Add(item);
            owner.Remove(item, 1);

            return true;
        }
    }
}