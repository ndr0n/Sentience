using Sentience;
using Unity.Entities;
using UnityEngine;

namespace MindTheatre
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "I_Speak", menuName = "Scaerth/Interaction/Speak")]
    public class I_Speak : Interaction
    {
        public override bool HasInteraction(EntityData self, EntityData interactor, EntityData target)
        {
            if (!self.Has<Persona>()) return false;

            if (!interactor.Has<Identity>()) return false;

            if (self.IsHostile(interactor)) return false;

            return true;
        }

        public override bool Interact(ref SystemState state, RefRW<InteractionComponent> comp)
        {
            Body body = state.EntityManager.GetComponentObject<Body>(comp.ValueRO.Self);
            if (body.Spawn == null) return false;

            Speaker speaker = body.Spawn.GetComponent<Speaker>();
            if (speaker == null) return false;

            Identity identity = state.EntityManager.GetComponentObject<Identity>(comp.ValueRO.Interactor);
            speaker.StartSpeakingWith(identity);

            return true;
        }
    }
}