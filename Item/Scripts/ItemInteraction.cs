using UnityEngine;

namespace Sentience
{
    public abstract class ItemInteraction : ScriptableObject
    {
        public string Name = "Item Interaction";
        public string Description = "a description of this interaction";

        public bool CanExecute(Item item, IdentityData owner, IdentityData interactor)
        {
            return OnCanExecute(item, owner, interactor);
        }

        protected abstract bool OnCanExecute(Item item, IdentityData owner, IdentityData interactor);

        public bool TryExecute(Item item, IdentityData owner, IdentityData interactor)
        {
            if (OnTryExecute(item, owner, interactor))
            {
                return true;
            }
            return false;
        }

        protected abstract bool OnTryExecute(Item item, IdentityData owner, IdentityData interactor);
    }
}