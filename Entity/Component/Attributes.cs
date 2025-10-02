using System;
using MindTheatre;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

namespace Sentience
{
    [System.Serializable]
    public class Attributes : EntityComponent
    {
        public int Strength;
        public int Dexterity;
        public int Agility;
        public int Intelligence;
        public int Wisdom;
        public int Charisma;

        [Header("Stats")]
        public int Armor;
        public int Evasion;

        public int Speed;
        public int Vision;

        public int Melee;
        public int Ranged;
        public int Physics;
    }

    [System.Serializable]
    public class AttributesAuthoring : EntityAuthoring
    {
        public Vector2Int Strength = new Vector2Int(1, 20);
        public Vector2Int Dexterity = new Vector2Int(1, 20);
        public Vector2Int Agility = new Vector2Int(1, 20);
        public Vector2Int Intelligence = new Vector2Int(1, 20);
        public Vector2Int Wisdom = new Vector2Int(1, 20);
        public Vector2Int Charisma = new Vector2Int(1, 20);

        public override IComponentData Spawn(Random random)
        {
            Attributes attributes = new();
            attributes.Strength = random.Next(Strength.x, Strength.y);
            attributes.Dexterity = random.Next(Dexterity.x, Dexterity.y);
            attributes.Agility = random.Next(Agility.x, Agility.y);
            attributes.Intelligence = random.Next(Intelligence.x, Intelligence.y);
            attributes.Wisdom = random.Next(Wisdom.x, Wisdom.y);
            attributes.Charisma = random.Next(Charisma.x, Charisma.y);
            return attributes;
        }
    }
}