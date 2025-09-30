using Sentience;
using Unity.Entities;
using UnityEngine;

namespace MindTheatre
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "I_Speak", menuName = "Scaerth/Interaction/Speak")]
    public class I_Speak : Interaction
    {
        public override bool HasInteraction(Entity self, Entity interactor, Entity target)
        {
            if (!EntityLibrary.Has<Persona>(self)) return false;

            if (!EntityLibrary.Has<Identity>(interactor)) return false;

            if (EntityLibrary.IsHostile(self, interactor)) return false;

            return true;
        }

        protected override bool OnTryInteract(Entity self, Entity interactor, Entity target)
        {
            Body body = EntityLibrary.Get<Body>(self);
            if (body.Spawn == null) return false;

            Speaker speaker = body.Spawn.GetComponent<Speaker>();
            if (speaker == null) return false;

            Identity identity = EntityLibrary.Get<Identity>(interactor);
            speaker.StartSpeakingWith(identity);

            return true;
        }
    }
}