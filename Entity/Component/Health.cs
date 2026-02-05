using System;
using System.Numerics;
using MindTheatre;
using UnityEngine;
using Random = System.Random;

namespace Sentience
{
    [System.Serializable]
    public class Health : EntityComponent
    {
        [SerializeField] float val;

        public float Value
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
        public float HealthRegeneration = 0;

        public Action<float> OnHealthChanged;
        public Action<float, EntityData> OnTakeDamage;
        public Action<EntityData, EntityData> OnHealthDepleted;

        public float TakeDamage(float dmg, EntityData attacker)
        {
            Debug.Log($"{Data.Name} took {dmg} damage!");
            if (dmg <= 0) return dmg;

            Value -= dmg;
            OnTakeDamage?.Invoke(dmg, attacker);

            if (Value <= 0) OnHealthDepleted?.Invoke(Data, attacker);

            return dmg;
        }
    }

    [System.Serializable]
    public class HealthAuthoring : EntityAuthoring
    {
        public Vector2Int MaxHealth = new Vector2Int(100, 100);
        public float HealthRegeneration = 0;

        public override IEntityComponent Spawn(Random random)
        {
            Health health = new();
            health.MaxHealth = random.Next(MaxHealth.x, MaxHealth.y);
            health.Value = health.MaxHealth;
            health.HealthRegeneration = HealthRegeneration;
            return health;
        }
    }
}