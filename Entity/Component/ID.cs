using MindTheatre;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace Sentience
{
    [System.Serializable]
    public class ID : Component
    {
        public Faction Faction;
        public string Location = "";

        public ID(EntityData data, EntityType entityType, string location) : base(data)
        {
            _data = data;
            Location = location;
            Inventory inv = Data.Get<Inventory>();
            if (inv == null)
            {
                inv = new Inventory(data);
                data.Components.Add(new(inv));
            }
        }
    }
}