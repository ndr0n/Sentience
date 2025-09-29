using System.Collections.Generic;
using Sentience;
using UnityEngine;
using Random = System.Random;

namespace MindTheatre
{
    [System.Serializable]
    public class Avatar : EntityComponent
    {
        public Sprite Sprite;
    }

    [System.Serializable]
    public class AvatarAuthoring : EntityComponentAuthoring
    {
        public List<Sprite> PossibleSprites = new();

        public override IEntityComponent Spawn(Random random)
        {
            Avatar avatar = new();
            avatar.Sprite = PossibleSprites[random.Next(PossibleSprites.Count)];
            return avatar;
        }
    }
}