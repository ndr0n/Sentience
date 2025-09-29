using Unity.Entities;

namespace Sentience
{
    [System.Serializable]
    public class EntityComponent : IEntityComponent
    {
        protected EntityData _data;
        public EntityData Data => _data;

        public virtual void Init(EntityData data, System.Random random)
        {
            _data = data;
        }
    }
}