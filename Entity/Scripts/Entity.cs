using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    public abstract class Entity : MonoBehaviour
    {
        public EntityType Type;
        [SerializeReference] public EntityData Data = null;
    }
}