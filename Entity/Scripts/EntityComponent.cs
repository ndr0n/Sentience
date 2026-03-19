using UnityEngine;
using Random = System.Random;

namespace Sentience
{
    public class EntityComponent
    {
        [HideInInspector] [SerializeReference] public EntityData Data;

        public virtual void OnInit(EntityData data, Random random)
        {
            Data = data;
        }

        public virtual void OnSpawn(EntitySpawn spawn)
        {
        }
    }
}