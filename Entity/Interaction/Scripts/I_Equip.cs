using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "I_Equip", menuName = "Sentience/Interaction/Equip")]
    public class I_Equip : Interaction
    {
        public override bool HasInteraction(EntityData self, EntityData interactor, EntityData target)
        {
            if (!self.Has<Weapon>()) return false;
            if (!interactor.Has<Equipment>()) return false;
            if (!interactor.Has<Inventory>()) return false;

            return true;
        }

        protected override bool OnTryInteract(EntityData self, EntityData interactor, EntityData target)
        {
            Weapon weapon = self.Get<Weapon>();
            Equipment equipment = interactor.Get<Equipment>();
            Inventory inventory = interactor.Get<Inventory>();
            equipment.EquipWeapon(weapon, inventory);

            return true;
        }
    }
}