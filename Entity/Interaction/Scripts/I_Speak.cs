using Sentience;
using UnityEngine;

namespace MindTheatre
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "I_Speak", menuName = "Scaerth/Interaction/Speak")]
    public class I_Speak : EntityInteraction
    {
        protected override bool IsPossible(Body self, EntityData interactor, EntityData target)
        {
            if (self.Data.IsHostile(interactor)) return false;
            return true;
        }

        protected override bool TryExecute(Body self, EntityData interactor, EntityData target)
        {
            Persona persona = interactor.Get<Persona>();
            if (persona == null) return false;
            Speaker speaker = self.Entity.GetComponent<Speaker>();
            if (speaker == null) return false;
            speaker.StartSpeakingWith(persona);
            return true;
        }
    }
}