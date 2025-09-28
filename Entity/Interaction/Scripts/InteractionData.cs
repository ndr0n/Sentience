using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    [System.Serializable]
    public class InteractionData
    {
        public Interaction Interaction;
        public string Item;
        public string Target;

        public InteractionData(string item, string target, Interaction interaction)
        {
            Item = item;
            Target = target;
            Interaction = interaction;
        }
    }
}