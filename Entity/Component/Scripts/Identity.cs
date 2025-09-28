using MindTheatre;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace Sentience
{
    [System.Serializable]
    public class Identity : IEntityComponent
    {
        public Faction Faction;
        public string Location;

        EntityData _data;
        public EntityData Data => _data;

        public void Init(EntityData data, System.Random random)
        {
            _data = data;
        }
    }

    [System.Serializable]
    public class IdentityAuthoring : EntityComponentAuthoring
    {
        public Faction Faction;
        public string Location;

        public override IEntityComponent Spawn(Random random)
        {
            Identity identity = new Identity();
            identity.Faction = Faction;
            identity.Location = Location;
            return identity;
        }
    }
}