using System;
using MindTheatre;
using Unity.VisualScripting;
using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    public class Lock : EntityComponent
    {
        public override void OnInit(EntityData data, System.Random random)
        {
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