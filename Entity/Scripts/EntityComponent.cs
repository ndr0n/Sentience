using UnityEngine;
using Random = System.Random;

namespace Sentience
{
    public class EntityComponent : IEntityComponent
    {
        EntityData _data;
        public EntityData Data => _data;

        public virtual void OnInit(EntityData data, Random random)
        {
            _data = data;
        }

        public virtual void OnSpawn(EntitySpawn spawn)
        {
        }
    }
}