using System;
using Sentience;
using Unity.Entities;

namespace Sentience
{
    public class Equipment : EntityComponent
    {
        public EntityData MeleeWeapon;
        public EntityData RangedWeapon;
        public Action<Equipment> OnEquipmentChanged;

        public void EquipWeapon(Weapon weapon, Inventory inventory)
        {
            switch (weapon.Class)
            {
                case WeaponClass.Melee:
                    if (MeleeWeapon != null)
                    {
                        Item existingWeapon = MeleeWeapon.Get<Item>();
                        inventory.Add(existingWeapon);
                        MeleeWeapon = null;
                    }
                    Item meleeWeapon = weapon.Data.Get<Item>();
                    inventory.Remove(meleeWeapon, 1);
                    MeleeWeapon = weapon.Data;
                    break;
                case WeaponClass.Ranged:
                    if (RangedWeapon != null)
                    {
                        Item existingWeapon = RangedWeapon.Get<Item>();
                        inventory.Add(existingWeapon);
                        RangedWeapon = null;
                    }
                    Item rangedWeapon = weapon.Data.Get<Item>();
                    inventory.Remove(rangedWeapon, 1);
                    RangedWeapon = weapon.Data;
                    break;
            }
            OnEquipmentChanged?.Invoke(this);
        }
    }

    [System.Serializable]
    public class EquipmentAuthoring : EntityAuthoring
    {
        public EntityType MeleeWeapon;
        public EntityType RangedWeapon;

        public override IComponentData Spawn(System.Random random)
        {
            Equipment equipment = new();

            if (MeleeWeapon != null)
            {
                EntityData meleeWeaponData = new(MeleeWeapon.Name, MeleeWeapon.Description, MeleeWeapon, random);
                // Item meleeWeaponItem = meleeWeaponData.Get<Item>();
                Weapon meleeWeapon = meleeWeaponData.Get<Weapon>();
                equipment.MeleeWeapon = meleeWeapon.Data;
            }

            if (RangedWeapon != null)
            {
                EntityData rangedWeaponData = new(RangedWeapon.Name, RangedWeapon.Description, RangedWeapon, random);
                // Item meleeWeaponItem = meleeWeaponData.Get<Item>();
                Weapon rangedWeapon = rangedWeaponData.Get<Weapon>();
                equipment.RangedWeapon = rangedWeapon.Data;
            }

            return equipment;
        }
    }
}