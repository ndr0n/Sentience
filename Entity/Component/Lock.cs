using System;
using MindTheatre;
using Unity.VisualScripting;
using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    public class Lock : Component
    {
        public Lock(EntityData data) : base(data)
        {
            _data = data;
        }
        
        [SerializeField] bool locked = false;
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
}