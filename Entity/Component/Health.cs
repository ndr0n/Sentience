using System;
using System.Numerics;
using Unity.Entities;
using UnityEngine;
using Random = System.Random;

namespace Sentience
{
    [System.Serializable]
    public class Health : EntityComponent
    {
        [SerializeField] int val;
        public int Value
        {
            get => val;
            set
            {
                val = value;

                if (val < 0) val = 0;
                else if (val > MaxHealth) val = MaxHealth;

                OnHealthChanged?.Invoke(val);
            }
        }

        public int MaxHealth;
        public Action<int> OnHealthChanged;
    }

    [System.Serializable]
    public class HealthAuthoring : EntityAuthoring
    {
        public Vector2Int Health = new Vector2Int(100, 100);
        public Vector2Int MaxHealth = new Vector2Int(100, 100);

        public override IEntityComponent Spawn(Random random)
        {
            Health health = new();
            health.Value = random.Next(Health.x, Health.y);
            health.MaxHealth = random.Next(MaxHealth.x, MaxHealth.y);
            return health;
        }
    }
}