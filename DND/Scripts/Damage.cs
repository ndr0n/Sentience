using UnityEngine;

namespace Sentience
{
    public enum DamageType
    {
        Physical,
        Physics,
    }

    [System.Serializable]
    public struct Damage
    {
        public int Value;
        public DamageType Type;
        public GameObject Projectile;

        public Damage(int value, DamageType type, GameObject projectile)
        {
            Type = type;
            Value = value;
            Projectile = projectile;
        }
    }
}