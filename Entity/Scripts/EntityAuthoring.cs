using Unity.Entities;

namespace Sentience
{
    public abstract class EntityAuthoring
    {
        public abstract IComponentData Spawn(System.Random random);
    }
}