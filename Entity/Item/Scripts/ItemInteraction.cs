using UnityEngine;

namespace Sentience
{
    public abstract class ItemInteraction : Interaction
    {
        public Vector2Int Range = new Vector2Int(0, 1);

        protected override bool OnTryInteract(EntityData self, EntityData interactor, EntityData target)
        {
            if (!IsPossible(self)) return false;
            if (!HasInteraction(self, interactor, target)) return false;
            Item item = self as Item;
            return TryExecute(item, interactor, target);
        }

        public override bool HasInteraction(EntityData self, EntityData interactor, EntityData target)
        {
            if (self is not Item item) return false;
            return OnHasInteraction(item, interactor);
        }

        protected abstract bool OnHasInteraction(Item self, EntityData interactor);

        protected abstract bool TryExecute(Item self, EntityData interactor, EntityData target);
    }
}