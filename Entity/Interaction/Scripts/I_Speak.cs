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
            if (!self.Has<Speaker>()) return false;

            if (!interactor.Has<Identity>()) return false;

            if (self.IsHostile(interactor)) return false;

            return true;
        }

        protected override bool OnTryInteract(EntityData self, EntityData interactor, EntityData target)
        {
            Speaker speaker = self.Get<Speaker>();
            Identity identity = interactor.Get<Identity>();

            if (identity.Data.Has<Player>())
            {
                Hud.Instance.DialogPanel.Show(true, speaker);
                speaker.StartSpeakingWith(identity);
            }
            
            return true;
        }
    }
}