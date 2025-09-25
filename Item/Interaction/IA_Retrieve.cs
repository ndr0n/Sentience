using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "IA_Retrieve", menuName = "Sentience/Item/Interaction/Retrieve")]
    public class IA_Retrieve : ItemInteraction
    {
        protected override bool OnCanExecute(Item item, IdentityData owner, IdentityData interactor)
        {
            if (interactor.Inventory == null) return false;
            return true;
        }

        protected override bool OnTryExecute(Item item, IdentityData owner, IdentityData interactor)
        {
            interactor.Inventory.Add(item);
            owner.Inventory.Remove(item, 1);
            return true;
        }
    }
}