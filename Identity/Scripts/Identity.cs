using UnityEngine;

namespace Sentience
{
    public abstract class Identity : MonoBehaviour
    {
        public IdentityType Type;
        [SerializeReference] public IdentityData Data = null;

        public virtual bool IsHostile(Identity identity)
        {
            if (Data.Faction == null) return false;
            if (identity.Data.Faction == null) return false;
            return Data.Faction.IsHostile(identity.Data.Faction);
        }
    }
}