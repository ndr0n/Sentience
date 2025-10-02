using Sentience;
using UnityEngine;

namespace MindTheatre
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "I_Explode", menuName = "Sentience/Interaction/Explode")]
    public class I_Explode : Interaction
    {
        public override bool HasInteraction(EntityData self, EntityData interactor, EntityData target)
        {
            if (!self.Has<Explosive>()) return false;
            return true;
        }

        protected override bool OnTryInteract(EntityData self, EntityData interactor, EntityData target)
        {
            ID id = self.GetData<ID>();
            Explosive explosive = self.Get<Explosive>();

            Vector3 worldPosition = new Vector3(id.Position.x, id.Position.z, id.Position.y);

            explosive.Explode(worldPosition);

            return true;
        }
    }
}