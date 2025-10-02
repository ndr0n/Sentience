using Sentience;

namespace Sentience
{
    public interface IEntityComponent
    {
        public EntityData Data { get; }
        public void Init(EntityData data, System.Random random);
    }
}