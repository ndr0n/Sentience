using System;
using System.Collections.Generic;
using MindTheatre;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    [System.Serializable]
    public class Explosive : EntityComponent
    {
        public int Range = 3;
        public Damage Damage;

        public void Explode(Vector3 worldPosition)
        {
            foreach (var Actor in GameManager.Instance.Actor)
            {
                float distance = Vector3.Distance(worldPosition, Actor.transform.position);
                if (distance < Range)
                {
                    Debug.Log($"EXPLOSIVE DAMAGE {Actor.name}");
                    var awaiter = Actor.TakeDamage(Damage, null);
                }
            }
        }
    }

    [System.Serializable]
    public class ExplosiveAuthoring : EntityAuthoring
    {
        public Vector2Int Range = new Vector2Int(3, 3);
        public Vector2Int Damage = new Vector2Int(10, 20);
        public DamageType DamageType = DamageType.Physics;

        public override IEntityComponent Spawn(System.Random random)
        {
            Explosive explosive = new()
            {
                Range = random.Next(Range.x, Range.y + 1),
                Damage = new Damage(random.Next(Damage.x, Damage.y + 1), DamageType, null),
            };
            return explosive;
        }
    }
}