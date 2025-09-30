using Sentience;
using Unity.Entities;

namespace Sentience
{
    public interface IEntityComponent : IComponentData
    {
        public Entity Entity { get; }
        public void Init(Entity entity, System.Random random);
    }
}