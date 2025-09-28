using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    public abstract class EntityComponent
    {
        protected EntityData _data;
        public EntityData Data => _data;

        public void Init(EntityData data, System.Random random)
        {
            _data = data;
            OnInit(data, random);
        }

        public abstract void OnInit(EntityData data, System.Random random);
    }
}