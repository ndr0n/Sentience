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
        // public RandomAudio Audio;
        public GameObject Animation;
        public int Radius = 5;
        public int Force = 3;
        public Damage Damage;

        public async Awaitable Explode(Vector3 worldPosition)
        {
            try
            {
                // GameObject animation = UnityEngine.Object.Instantiate(Animation, worldPosition, Quaternion.identity);
                // Audio.Play();
                // Vector3 radius = new Vector3(Radius, Radius, Radius);
                // animation.transform.localScale = radius;
                // Bounds bounds = new Bounds(worldPosition, radius);
                //
                // List<Body> affected = BodyLibrary.GetBodiesInBounds(bounds);
                // await Awaitable.WaitForSecondsAsync(0.125f);
                //
                // foreach (var body in affected)
                // {
                //     Vector3 direction = body.Data.ID.Position - worldPosition;
                //     body.Push(Vector3Int.RoundToInt(direction.normalized), Force);
                //     await body.TakeDamage(Damage, null);
                // }
                //
                // await Awaitable.WaitForSecondsAsync(0.125f);
                // UnityEngine.Object.Destroy(animation);
                //
                // foreach (var Actor in BodyManager.Instance.Bodies)
                // {
                //     if (Actor.Data.Has<ID>())
                //     {
                //         ID id = Actor.Data.Get<ID>();
                //         float distance = Vector3.Distance(worldPosition, id.Position);
                //         if (distance <= Radius)
                //         {
                //             Debug.Log($"EXPLOSIVE DAMAGE {Actor.Data.Name}");
                //             // var awaiter = Actor.TakeDamage(Damage, null);
                //         }
                //     }
                // }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }

    [System.Serializable]
    public class ExplosiveAuthoring : EntityAuthoring
    {
        // public RandomAudio Audio;
        public GameObject Animation;
        public Vector2Int Force = new Vector2Int(4, 4);
        public Vector2Int Radius = new Vector2Int(4, 4);
        public Vector2Int Damage = new Vector2Int(10, 20);
        public DamageType DamageType = DamageType.Physics;

        public override IEntityComponent Spawn(System.Random random)
        {
            Explosive explosive = new()
            {
                // Audio = Audio,
                Animation = Animation,
                Force = random.Next(Force.x, Force.y + 1),
                Radius = random.Next(Radius.x, Radius.y + 1),
                Damage = new Damage(random.Next(Damage.x, Damage.y + 1), DamageType, null),
            };
            return explosive;
        }
    }
}