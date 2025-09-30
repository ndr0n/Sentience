using Unity.Entities;
using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "I_Give", menuName = "Sentience/Interaction/Give")]
    public class I_Give : Interaction
    {
        public override bool HasInteraction(Entity self, Entity interactor, Entity target)
        {
            if (interactor == target) return false;

            if (!EntityLibrary.Has<Item>(self)) return false;

            if (!EntityLibrary.Has<Inventory>(interactor)) return false;
            Inventory giver = EntityLibrary.Get<Inventory>(interactor);
            if (!giver.Items.Exists(x => x.Item.Entity == self)) return false;

            if (!EntityLibrary.Has<Inventory>(target)) return false;

            return true;
        }

        protected override bool OnTryInteract(Entity self, Entity interactor, Entity target)
        {
            Item item = EntityLibrary.Get<Item>(self);
            Inventory giver = EntityLibrary.Get<Inventory>(interactor);
            Inventory receiver = EntityLibrary.Get<Inventory>(target);

            receiver.Add(item);
            giver.Remove(item, 1);

            return true;
        }
    }
}