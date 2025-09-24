using UnityEngine;

namespace Sentience
{
    public abstract class Identity : MonoBehaviour
    {
        public IdentityType Type;
        public IdentityData Data = null;
    }
}