using System;

namespace Sentience
{
    [System.Serializable]
    public class Attributes : EntityComponent
    {
        public int Strength = 10;
        public int Dexterity = 10;
        public int Constitution = 10;
        public int Intelligence = 10;
        public int Wisdom = 10;
        public int Charisma = 10;

        public override void OnInit(EntityData data, Random random)
        {
        }
    }
}