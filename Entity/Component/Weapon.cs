using System.Numerics;
using Sentience;
using UnityEngine;

namespace Sentience
{
    public enum WeaponClass
    {
        Melee,
        Ranged,
    }

    [System.Serializable]
    public class Weapon : EntityComponent
    {
        public int Damage = 1;
        public WeaponClass Class;
        public SpawnAction Action;
    }

    [System.Serializable]
    public class WeaponAuthoring : EntityAuthoring
    {
        public Vector2Int Damage = new Vector2Int(1, 1);
        public WeaponClass Class;
        public SpawnAction Action;

        public override IEntityComponent Spawn(System.Random random)
        {
            Weapon weapon = new();
            weapon.Damage = random.Next(Damage.x, Damage.y + 1);
            weapon.Class = Class;
            weapon.Action = Action;
            return weapon;
        }
    }
}