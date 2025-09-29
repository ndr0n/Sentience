using System.Collections.Generic;
using Sentience;
using UnityEngine;
using Random = System.Random;

namespace MindTheatre
{
    [System.Serializable]
    public class Avatar : IEntityComponent
    {
        public Sprite Sprite;

        EntityData _data;
        public EntityData Data => _data;

        public void Init(EntityData data, Random random)
        {
            _data = data;
        }
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