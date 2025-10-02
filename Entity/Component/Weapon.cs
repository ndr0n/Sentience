using System.Numerics;
using Sentience;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

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
        [FormerlySerializedAs("Action")]
        public EntityAction EntityAction;
    }

    [System.Serializable]
    public class WeaponAuthoring : EntityAuthoring
    {
        public Vector2Int Damage = new Vector2Int(1, 1);
        public WeaponClass Class;
        [FormerlySerializedAs("Action")]
        public EntityAction EntityAction;

        public override IComponentData Spawn(System.Random random)
        {
            Weapon weapon = new();
            weapon.Damage = random.Next(Damage.x, Damage.y + 1);
            weapon.Class = Class;
            weapon.EntityAction = EntityAction;
            return weapon;
        }
    }
}