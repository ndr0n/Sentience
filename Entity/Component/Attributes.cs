using System;
using UnityEngine;
using Random = System.Random;

namespace Sentience
{
    [System.Serializable]
    public class Attributes : EntityComponent
    {
        public int Strength;
        public int Dexterity;
        public int Constitution;
        public int Intelligence;
        public int Wisdom;
        public int Charisma;

        [Header("Stats")]
        public int Speed;
        public int MeleeDamage;
        public int RangedDamage;
        public int PhysicsDamage;
        public int Armor;
        public int Poise;
        public int Willpower;
    }

    [System.Serializable]
    public class AttributesAuthoring : EntityAuthoring
    {
        public Vector2Int Strength = new Vector2Int(1, 20);
        public Vector2Int Dexterity = new Vector2Int(1, 20);
        public Vector2Int Constitution = new Vector2Int(1, 20);
        public Vector2Int Intelligence = new Vector2Int(1, 20);
        public Vector2Int Wisdom = new Vector2Int(1, 20);
        public Vector2Int Charisma = new Vector2Int(1, 20);

        public override IEntityComponent Spawn(Random random)
        {
            Attributes attributes = new();
            attributes.Strength = random.Next(Strength.x, Strength.y);
            attributes.Dexterity = random.Next(Dexterity.x, Dexterity.y);
            attributes.Constitution = random.Next(Constitution.x, Constitution.y);
            attributes.Intelligence = random.Next(Intelligence.x, Intelligence.y);
            attributes.Wisdom = random.Next(Wisdom.x, Wisdom.y);
            attributes.Charisma = random.Next(Charisma.x, Charisma.y);
            return attributes;
        }
    }
}