using System;
using Sentience;

namespace Sentience
{
    public class Equipment : EntityComponent
    {
        public Weapon MeleeWeapon;
        public Weapon RangedWeapon;
        public Action<Equipment> OnEquipmentChanged;

        public void EquipWeapon(Weapon weapon, Inventory inventory)
        {
            switch (weapon.Class)
            {
                case WeaponClass.Melee:
                    if (MeleeWeapon != null)
                    {
                        Item existingWeapon = MeleeWeapon.Data.Get<Item>();
                        inventory.Add(existingWeapon);
                        MeleeWeapon = null;
                    }
                    Item meleeWeapon = weapon.Data.Get<Item>();
                    inventory.Remove(meleeWeapon, 1);
                    MeleeWeapon = weapon;
                    break;
                case WeaponClass.Ranged:
                    if (RangedWeapon != null)
                    {
                        Item existingWeapon = RangedWeapon.Data.Get<Item>();
                        inventory.Add(existingWeapon);
                        RangedWeapon = null;
                    }
                    Item rangedWeapon = weapon.Data.Get<Item>();
                    inventory.Remove(rangedWeapon, 1);
                    RangedWeapon = weapon;
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

        public override IEntityComponent Spawn(System.Random random)
        {
            Equipment equipment = new();

            if (MeleeWeapon != null)
            {
                EntityData meleeWeaponData = new(MeleeWeapon.Name, MeleeWeapon.Description, MeleeWeapon, random);
                // Item meleeWeaponItem = meleeWeaponData.Get<Item>();
                Weapon meleeWeapon = meleeWeaponData.Get<Weapon>();
                equipment.MeleeWeapon = meleeWeapon;
            }

            if (RangedWeapon != null)
            {
                EntityData rangedWeaponData = new(RangedWeapon.Name, RangedWeapon.Description, RangedWeapon, random);
                // Item meleeWeaponItem = meleeWeaponData.Get<Item>();
                Weapon rangedWeapon = rangedWeaponData.Get<Weapon>();
                equipment.RangedWeapon = rangedWeapon;
            }

            return equipment;
        }
    }
}