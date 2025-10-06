using Sentience;
using UnityEngine;

namespace Sentience
{
    public interface IEntityComponent
    {
        public EntityData Data { get; }
        public void OnInit(EntityData data, System.Random random);
        public void OnSpawn(EntitySpawn spawn);
    }
}