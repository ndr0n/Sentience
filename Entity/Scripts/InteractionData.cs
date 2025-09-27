using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    [System.Serializable]
    public class InteractionData
    {
        public Interaction Interaction;
        public string Target;

        public InteractionData(string target, Interaction interaction)
        {
            Target = target;
            Interaction = interaction;
        }
    }
}