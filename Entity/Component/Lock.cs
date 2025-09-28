using System;
using MindTheatre;
using Unity.VisualScripting;
using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    public class Lock : IEntityComponent
    {
        EntityData _data;
        public EntityData Data => _data;

        public void Init(EntityData data, System.Random random)
        {
            _data = data;
        }

        [SerializeField] bool locked;
        public bool Locked
        {
            get => locked;
            set
            {
                if (value != locked)
                {
                    locked = value;
                    OnLockStateChanged?.Invoke(locked);
                }
            }
        }

        public Action<bool> OnLockStateChanged;
    }

    [System.Serializable]
    public class LockAuthoring : EntityComponentAuthoring
    {
        public bool Locked = false;

        public override IEntityComponent Spawn(System.Random random)
        {
            Lock lockComponent = new();
            lockComponent.Locked = Locked;
            return lockComponent;
        }
    }
}