using System;
using MindTheatre;
using UnityEngine;

namespace Sentience
{
    public class Lock
    {
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