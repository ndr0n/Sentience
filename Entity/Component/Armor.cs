using System.Numerics;
using MindTheatre;
using Sentience;
using Unity.Entities;
using UnityEngine;

namespace Sentience
{
    public enum ArmorClass
    {
        Head,
        Chest,
        Hands,
        Feet,
        Ring1,
        Ring2,
    }

    [System.Serializable]
    public class Armor : EntityComponent
    {
        public int Protection = 1;
        public ArmorClass Class;
        public Effect Effect;
    }

    [System.Serializable]
    public class ArmorAuthoring : EntityAuthoring
    {
        public Vector2Int Protection = new Vector2Int(1, 1);
        public ArmorClass Class;
        public Effect Effect;

        public override IComponentData Spawn(System.Random random)
        {
            Armor armor = new();
            armor.Protection = random.Next(Protection.x, Protection.y + 1);
            armor.Class = Class;
            armor.Effect = Effect;
            return armor;
        }
    }
}