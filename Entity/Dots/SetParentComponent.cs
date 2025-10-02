using Unity.Entities;

namespace Sentience
{
    public struct SetParentComponent : IComponentData
    {
        public Entity Parent;
    }
}