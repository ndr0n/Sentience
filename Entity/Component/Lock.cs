using System;
using System.Collections.Generic;
using MindTheatre;
using Unity.VisualScripting;
using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    public class Lock : EntityComponent
    {
        public List<int> KeyCode = new();
        public List<EntityData> KeyItems = new();

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
                    Open = value;
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
        public List<int> KeyCode = new();
        public List<EntityType> KeyItems = new();
        public bool Open = false;
        public bool Locked = false;

        public override IEntityComponent Spawn(System.Random random)
        {
            Lock lockComponent = new();
            lockComponent.KeyItems = new();
            foreach (var key in KeyItems) lockComponent.KeyItems.Add(new(key.Name, key.Description, key, random));
            lockComponent.Open = Open;
            lockComponent.Locked = Locked;
            lockComponent.KeyCode = KeyCode;
            return lockComponent;
        }
    }
}