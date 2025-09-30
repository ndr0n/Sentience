using System;
using Unity.Entities;

namespace Sentience
{
    public class EntityComponent : IEntityComponent
    {
        Entity _entity;
        public Entity Entity => _entity;

        public virtual void Init(Entity entity, Random random)
        {
            _entity = entity;
        }
    }
}