using Sentience;
using UnityEngine;

namespace MindTheatre
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "I_Speak", menuName = "Scaerth/Interaction/Speak")]
    public class I_Speak : Interaction
    {
        public override bool HasInteraction(EntityData self, EntityData interactor, EntityData target)
        {
            Persona persona = self.Get<Persona>();
            if (persona == null) return false;
            if (self.IsHostile(interactor)) return false;

            Identity identity = interactor.Get<Identity>();
            if (identity == null) return false;

            return true;
        }

        protected override bool OnTryInteract(EntityData self, EntityData interactor, EntityData target)
        {
            Body body = self.Get<Body>();
            if (body == null) return false;
            if (body.Entity == null) return false;

            Speaker speaker = body.Entity.GetComponent<Speaker>();
            if (speaker == null) return false;

            Identity identity = interactor.Get<Identity>();
            speaker.StartSpeakingWith(identity);

            return true;
        }
    }
}