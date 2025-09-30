using System;

namespace Sentience
{
    public class EntityComponent : IEntityComponent
    {
        EntityData _data;
        public EntityData Data => _data;

        public virtual void Init(EntityData data, Random random)
        {
            _data = data;
        }
    }
}