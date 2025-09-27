using UnityEngine;

namespace Sentience
{
    public abstract class Identity : MonoBehaviour
    {
        public IdentityType Type;
        [SerializeReference] public IdentityData Data = null;
    }
}