using System;
using MindTheatre;
using Unity.VisualScripting;
using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    public class Lock : EntityComponent
    {
        [SerializeField] bool open;
        public bool Open
        {
            get => open;
            set
            {
                if (open != value)
                {
                    open = value;
                    OnOpenStateChanged?.Invoke(open);
                }
            }
        }

        public Action<bool> OnOpenStateChanged;

        [SerializeField] bool locked;
        public bool Locked
        {
            get => locked;
            set
            {
                if (locked != value)
                {
                    locked = value;
                    OnLockedStateChanged?.Invoke(locked);
                }
            }
        }

        public Action<bool> OnLockedStateChanged;
    }

    [System.Serializable]
    public class LockAuthoring : EntityAuthoring
    {
        public bool Open = false;
        public bool Locked = false;

        public override IEntityComponent Spawn(System.Random random)
        {
            Lock lockComponent = new();
            lockComponent.Open = Open;
            lockComponent.Locked = Locked;
            return lockComponent;
        }
    }
}