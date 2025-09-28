using MindTheatre;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace Sentience
{
    [System.Serializable]
    public class ID : EntityComponent
    {
        public Faction Faction;
        public string Location = "";

        public override void OnInit(EntityData data, System.Random random)
        {
        }
    }
}