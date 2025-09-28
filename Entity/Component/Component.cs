using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    public abstract class Component
    {
        protected EntityData _data;
        public EntityData Data => _data;

        protected Component(EntityData data)
        {
            _data = data;
        }
    }
}